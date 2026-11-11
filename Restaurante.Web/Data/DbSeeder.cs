using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.Models;
using Restaurante.Web.Services;

namespace Restaurante.Web.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            DataContext context,
            PasswordService passwordService)
        {
            await context.Database.MigrateAsync();

            var permisos = new[]
            {
                new Permission { Nombre = "Consultar usuarios", Codigo = "Usuarios.Ver" },
                new Permission { Nombre = "Crear usuarios", Codigo = "Usuarios.Crear" },
                new Permission { Nombre = "Editar usuarios", Codigo = "Usuarios.Editar" },

                new Permission { Nombre = "Consultar roles", Codigo = "Roles.Ver" },
                new Permission { Nombre = "Crear roles", Codigo = "Roles.Crear" },
                new Permission { Nombre = "Editar roles", Codigo = "Roles.Editar" },

                new Permission { Nombre = "Consultar cuentas", Codigo = "Cuentas.Ver" }
            };

            foreach (var permiso in permisos)
            {
                if (!await context.Permissions.AnyAsync(
                    x => x.Codigo == permiso.Codigo))
                {
                    context.Permissions.Add(permiso);
                }
            }

            await context.SaveChangesAsync();

            var administrador = await context.Roles
                .FirstOrDefaultAsync(x => x.Nombre == "Administrador");

            if (administrador == null)
            {
                administrador = new Rol
                {
                    Nombre = "Administrador",
                    Activo = true
                };

                context.Roles.Add(administrador);
                await context.SaveChangesAsync();
            }

            var todosLosPermisos = await context.Permissions.ToListAsync();

            foreach (var permiso in todosLosPermisos)
            {
                bool existe = await context.RolePermissions.AnyAsync(
                    x => x.Id_Rol == administrador.Id_Rol &&
                         x.Id_Permiso == permiso.Id_Permiso);

                if (!existe)
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        Id_Rol = administrador.Id_Rol,
                        Id_Permiso = permiso.Id_Permiso
                    });
                }
            }

            await context.SaveChangesAsync();

            var usuario = await context.Usuarios
                .FirstOrDefaultAsync(x => x.Email == "admin@restaurante.com");

            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Nombre = "Administrador",
                    Telefono = "3000000000",
                    Email = "admin@restaurante.com",
                    Id_Rol = administrador.Id_Rol,
                    Activo = true
                };

                context.Usuarios.Add(usuario);
                await context.SaveChangesAsync();

                var cuenta = new Cuenta
                {
                    Id_Usuario = usuario.Id_Usuario,
                    NombreUsuario = "admin",
                    PasswordHash = passwordService.HashPassword("Admin123*"),
                    Activa = true
                };

                context.Cuentas.Add(cuenta);
                await context.SaveChangesAsync();
            }
        }
    }
}
