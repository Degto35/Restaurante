using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.DTOs.Employees;
using Restaurante.Web.Entities;
using Restaurante.Web.Services.Abstractions;

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
                    IdEmployee = e.IdEmployee,
                    Name = e.Name,
                    Telephone = e.Telephone,
                    Email = e.Email,
                    IdRole = e.IdRole,
                    NameRole = e.Role.NameRole
                }).ToListAsync();
        }

        public async Task<EmployeesDTO?> ObtenerPorIdAsync(Guid id)
        {
            var employee = await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.IdEmployee == id);

            if (employee == null) return null;

            return new EmployeesDTO
            {
                IdEmployee = employee.IdEmployee,
                Name = employee.Name,
                Telephone = employee.Telephone,
                Email = employee.Email,
                IdRole = employee.IdRole,
                NameRole = employee.Role.NameRole
            };
        }

        public async Task<bool> CrearAsync(CreateEmployeeDTO dto)
        {
            var employee = new Employees
            {
                IdEmployee = Guid.NewGuid(),
                Name = dto.Name,
                Telephone = dto.Telephone,
                Email = dto.Email,
                IdRole = dto.IdRole
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
            employee.IdRole = dto.IdRole;

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
            return await _context.Role
                .OrderBy(r => r.NameRole)
                .Select(r => new SelectListItem
                {
                    Value = r.IdRole.ToString(),
                    Text = r.NameRole
                }).ToListAsync();
        }
    }
}