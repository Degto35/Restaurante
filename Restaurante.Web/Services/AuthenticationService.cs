using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.Models;
using System.Security.Claims;

namespace Restaurante.Web.Services
{
    public class AuthenticationService
    {
        private readonly DataContext _context;
        private readonly PasswordService _passwordService;

        public AuthenticationService(
            DataContext context,
            PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<Usuario?> AuthenticateAsync(
            string nombreUsuario,
            string password)
        {
            var cuenta = await _context.Cuentas
                .Include(x => x.Usuario)
                .ThenInclude(x => x!.Rol)
                .FirstOrDefaultAsync(x =>
                    x.NombreUsuario == nombreUsuario &&
                    x.Activa);

            if (cuenta == null || cuenta.Usuario == null)
                return null;

            if (!cuenta.Usuario.Activo)
                return null;

            if (!_passwordService.VerifyPassword(
                cuenta.PasswordHash,
                password))
                return null;

            return cuenta.Usuario;
        }

        public async Task<List<string>> ObtenerPermisosAsync(int idRol)
        {
            return await _context.RolePermissions
                .Where(x => x.Id_Rol == idRol)
                .Include(x => x.Permission)
                .Select(x => x.Permission!.Codigo)
                .ToListAsync();
        }
    }
}
