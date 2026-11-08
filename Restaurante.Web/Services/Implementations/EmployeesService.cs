using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.DTOs.Employees;
using Restaurante.Web.Services.Abstractions;
using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Services.Implementations
{
    public class EmployeesService : IEmployeesService
    {
        private readonly DataContext _context;

        public EmployeesService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeesDTO>> ObtenerTodosAsync()
        {
            return await _context.Employees
                .Select(e => new EmployeesDTO
                {
                    IdEmployee = e.Id,
                    Name = e.Name,
                    Telephone = e.Telephone,
                    Email = e.Email,
                    IdRole = e.RoleId,
                    NameRole = e.Role.Name
                }).ToListAsync();
        }

        public async Task<EmployeesDTO?> ObtenerPorIdAsync(Guid id)
        {
            var employee = await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return null;

            return new EmployeesDTO
            {
                IdEmployee = employee.Id,
                Name = employee.Name,
                Telephone = employee.Telephone,
                Email = employee.Email,
                IdRole = employee.RoleId,
                NameRole = employee.Role.Name
            };
        }

        public async Task<bool> CrearAsync(CreateEmployeeDTO dto)
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Telephone = dto.Telephone,
                Email = dto.Email,
                RoleId = dto.IdRole
            };

            _context.Employees.Add(employee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(UpdateEmployeeDTO dto)
        {
            var employee = await _context.Employees.FindAsync(dto.IdEmployee);
            if (employee == null) return false;

            employee.Name = dto.Name;
            employee.Telephone = dto.Telephone;
            employee.Email = dto.Email;
            employee.RoleId = dto.IdRole;

            _context.Employees.Update(employee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            // TODO: cuando existan Cuentas y Pedidos, validar que el empleado no tenga registros asociados

            _context.Employees.Remove(employee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<SelectListItem>> ObtenerRolesAsync()
        {
            return await _context.Roles
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                }).ToListAsync();
        }
    }
}