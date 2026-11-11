using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.DTOs.Employees;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class EmployeesService : CustomQueryableOperationsService, IEmployeesService
    {
        private readonly DataContext _context;

        public EmployeesService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }

        public async Task<Response<PaginationResponse<EmployeeDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            
            IQueryable<Employee> query = _context.Employees
                .Include(e => e.Role)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                string filter = request.Filter.ToLower();

                query = query.Where(e => e.Name.ToLower().Contains(filter)
                                      || (e.Email != null && e.Email.ToLower().Contains(filter))
                                      || e.Role.Name.ToLower().Contains(filter));
            }

            query = query.OrderBy(e => e.Name);

            return await GetPagedListAsync<EmployeeDTO, Employee>(request, query);
        }

        public async Task<Response<EmployeeDTO>> GetOneAsync(Guid id)
        {
            IQueryable<Employee> query = _context.Employees.Include(e => e.Role);

            return await GetOneAsync<EmployeeDTO, Employee>(id, query);
        }

        public async Task<Response<CreateEmployeeDTO>> CreateAsync(CreateEmployeeDTO dto)
        {
            if (!await _context.Roles.AnyAsync(r => r.Id == dto.RoleId))
            {
                return Response<CreateEmployeeDTO>.Failure("El rol seleccionado no existe.");
            }

            return await CreateAsync<CreateEmployeeDTO, Employee>(dto);
        }

        public async Task<Response<UpdateEmployeeDTO>> UpdateAsync(UpdateEmployeeDTO dto)
        {
            if (!await _context.Roles.AnyAsync(r => r.Id == dto.RoleId))
            {
                return Response<UpdateEmployeeDTO>.Failure("El rol seleccionado no existe.");
            }

            return await UpdateAsync<UpdateEmployeeDTO, Employee>(dto, dto.Id);
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            bool hasOrders = await _context.Orders.AnyAsync(o => o.EmployeeId == id);

            if (hasOrders)
            {
                return Response.Failure("No se puede eliminar el empleado porque tiene pedidos asociados.");
            }

            return await DeleteAsync<Employee>(id);
        }
    }
}