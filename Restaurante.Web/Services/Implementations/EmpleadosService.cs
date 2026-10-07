using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.DTOs.Empleados;
using Restaurante.Web.Entities;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class EmpleadosService : IEmpleadosService
    {
        private readonly DataContext _context;

        public EmpleadosService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<EmpleadoDTO>> ObtenerTodosAsync()
        {
            return await _context.Empleados
                .Select(e => new EmpleadoDTO
                {
                    IdEmpleado = e.IdEmpleado,
                    Nombre = e.Nombre,
                    Telefono = e.Telefono,
                    Email = e.Email,
                    IdRol = e.IdRol,
                    NombreRol = e.Rol.NombreRol
                }).ToListAsync();
        }

        public async Task<EmpleadoDTO?> ObtenerPorIdAsync(Guid id)
        {
            var empleado = await _context.Empleados
                .Include(e => e.Rol)
                .FirstOrDefaultAsync(e => e.IdEmpleado == id);

            if (empleado == null) return null;

            return new EmpleadoDTO
            {
                IdEmpleado = empleado.IdEmpleado,
                Nombre = empleado.Nombre,
                Telefono = empleado.Telefono,
                Email = empleado.Email,
                IdRol = empleado.IdRol,
                NombreRol = empleado.Rol.NombreRol
            };
        }

        public async Task<bool> CrearAsync(CrearEmpleadoDTO dto)
        {
            var empleado = new Empleado
            {
                IdEmpleado = Guid.NewGuid(),
                Nombre = dto.Nombre,
                Telefono = dto.Telefono,
                Email = dto.Email,
                IdRol = dto.IdRol
            };

            _context.Empleados.Add(empleado);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(ActualizarEmpleadoDTO dto)
        {
            var empleado = await _context.Empleados.FindAsync(dto.IdEmpleado);
            if (empleado == null) return false;

            empleado.Nombre = dto.Nombre;
            empleado.Telefono = dto.Telefono;
            empleado.Email = dto.Email;
            empleado.IdRol = dto.IdRol;

            _context.Empleados.Update(empleado);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(Guid id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return false;

            // TODO: cuando existan Cuentas y Pedidos, validar que el empleado no tenga registros asociados

            _context.Empleados.Remove(empleado);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<SelectListItem>> ObtenerRolesAsync()
        {
            return await _context.Roles
                .OrderBy(r => r.NombreRol)
                .Select(r => new SelectListItem
                {
                    Value = r.IdRol.ToString(),
                    Text = r.NombreRol
                }).ToListAsync();
        }
    }
}