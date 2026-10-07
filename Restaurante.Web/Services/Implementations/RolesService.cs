using global::Restaurante.Web.Data;
using global::Restaurante.Web.DTOs.Roles;
using global::Restaurante.Web.Entities;
using global::Restaurante.Web.Services.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace Restaurante.Web.Services.Implementations
{
    public class RolesService : IRolesService
    {
        private readonly DataContext _context;

        public RolesService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<RolDTO>> ObtenerTodosAsync()
        {
            return await _context.Roles
                .Select(r => new RolDTO { IdRol = r.IdRol, NombreRol = r.NombreRol })
                .ToListAsync();
        }

        public async Task<RolDTO?> ObtenerPorIdAsync(Guid id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return null;

            return new RolDTO { IdRol = rol.IdRol, NombreRol = rol.NombreRol };
        }

        public async Task<bool> CrearAsync(CrearRolDTO dto)
        {
            var rol = new Rol
            {
                IdRol = Guid.NewGuid(),
                NombreRol = dto.NombreRol
            };

            _context.Roles.Add(rol);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(ActualizarRolDTO dto)
        {
            var rol = await _context.Roles.FindAsync(dto.IdRol);
            if (rol == null) return false;

            rol.NombreRol = dto.NombreRol;

            _context.Roles.Update(rol);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(Guid id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return false;

            var tieneEmpleados = await _context.Empleados.AnyAsync(e => e.IdRol == id);
            if (tieneEmpleados) return false;

            _context.Roles.Remove(rol);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}