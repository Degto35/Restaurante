using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Data.Seeders
{
    public class PermissionsSeeder
    {
        private readonly DataContext _context;

        public PermissionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Permission> permissions =
            [
                ..Crud("categories", "Categorías", "Categorías"),
                ..Crud("dishes", "Platos", "Platos"),
                ..Crud("diningtables", "Mesas", "Mesas"),
                ..Crud("employees", "Empleados", "Empleados"),
                ..Crud("roles", "Roles", "Roles"),
                ..Orders()
            ];

            foreach (Permission permission in permissions)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.Code == permission.Code);

                if (!exists)
                {
                    await _context.Permissions.AddAsync(permission);
                }
            }

            await _context.SaveChangesAsync();
        }

        // Crea los 4 permisos básicos (ver, crear, editar, eliminar) de un módulo
        private static List<Permission> Crud(string prefix, string module, string plural)
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Code = $"{prefix}.show", Description = $"Ver {plural}", Module = module },
                new Permission { Id = Guid.NewGuid(), Code = $"{prefix}.create", Description = $"Crear {plural}", Module = module },
                new Permission { Id = Guid.NewGuid(), Code = $"{prefix}.update", Description = $"Editar {plural}", Module = module },
                new Permission { Id = Guid.NewGuid(), Code = $"{prefix}.delete", Description = $"Eliminar {plural}", Module = module },
            };
        }

        // Los pedidos no se eliminan, pero sí se cobran
        private static List<Permission> Orders()
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Code = "orders.show", Description = "Ver Pedidos", Module = "Pedidos" },
                new Permission { Id = Guid.NewGuid(), Code = "orders.create", Description = "Crear Pedidos", Module = "Pedidos" },
                new Permission { Id = Guid.NewGuid(), Code = "orders.update", Description = "Editar Pedidos", Module = "Pedidos" },
                new Permission { Id = Guid.NewGuid(), Code = "orders.pay", Description = "Cobrar Pedidos", Module = "Pedidos" },
            };
        }
    }
}