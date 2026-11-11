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
                .. Crud("categories", "Categorías", "categorías"),
                .. Crud("dishes", "Platos", "platos"),
                .. Crud("tables", "Mesas", "mesas"),
                .. Orders(),
                .. Crud("users", "Usuarios", "usuarios"),
                .. Crud("roles", "Roles", "roles"),
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

        private static List<Permission> Orders()
        {
            return new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Code = "orders.show", Description = "Ver pedidos", Module = "Pedidos" },
                new Permission { Id = Guid.NewGuid(), Code = "orders.create", Description = "Crear pedidos", Module = "Pedidos" },
                new Permission { Id = Guid.NewGuid(), Code = "orders.update", Description = "Editar pedidos (platos y estado)", Module = "Pedidos" },
                new Permission { Id = Guid.NewGuid(), Code = "orders.pay", Description = "Cobrar pedidos", Module = "Pedidos" },
            };
        }
    }
}