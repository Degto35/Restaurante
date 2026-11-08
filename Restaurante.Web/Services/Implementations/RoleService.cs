using global::Restaurante.Web.Data;
using global::Restaurante.Web.DTOs.Role;
using global::Restaurante.Web.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly DataContext _context;

        public RoleService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDTO>> ObtenerTodosAsync()
        {
            return await _context.Roles
                .Select(r => new RoleDTO { IdRole = r.Id, NameRole = r.Name })
                .ToListAsync();
        }

        public async Task<RoleDTO?> ObtenerPorIdAsync(Guid id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return null;

            return new RoleDTO { IdRole = rol.Id, NameRole = rol.Name };
        }

        public async Task<bool> CrearAsync(CreateRoleDTO dto)
        {
            var rol = new Role
            {
                Id = Guid.NewGuid(),
                Name = dto.NameRole
            };

            _context.Roles.Add(rol);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(UpdateRoleDTO dto)
        {
            var role = await _context.Roles.FindAsync(dto.IdRole);
            if (role == null) return false;

            role.Name = dto.NameRole;

            _context.Roles.Update(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            var tieneEmpleados = await _context.Employees.AnyAsync(e => e.RoleId == id);
            if (tieneEmpleados) return false;

            _context.Roles.Remove(role);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}