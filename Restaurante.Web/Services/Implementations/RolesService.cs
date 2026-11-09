using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.DTOs.Roles;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class RolesService : CustomQueryableOperationsService, IRolesService
    {
        private readonly DataContext _context;

        public RolesService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }

        public async Task<Response<PaginationResponse<RoleDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<Role> query = _context.Roles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                string filter = request.Filter.ToLower();
                query = query.Where(r => r.Name.ToLower().Contains(filter));
            }

            query = query.OrderBy(r => r.Name);

            return await GetPagedListAsync<RoleDTO, Role>(request, query);
        }

        public async Task<Response<RoleDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<RoleDTO, Role>(id);
        }

        public async Task<Response<CreateRoleDTO>> CreateAsync(CreateRoleDTO dto)
        {
            return await CreateAsync<CreateRoleDTO, Role>(dto);
        }

        public async Task<Response<UpdateRoleDTO>> UpdateAsync(UpdateRoleDTO dto)
        {
            return await UpdateAsync<UpdateRoleDTO, Role>(dto, dto.Id);
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            bool hasEmployees = await _context.Employees.AnyAsync(e => e.RoleId == id);

            if (hasEmployees)
            {
                return Response.Failure("No se puede eliminar el rol porque tiene empleados asociados.");
            }

            return await DeleteAsync<Role>(id);
        }
    }
}