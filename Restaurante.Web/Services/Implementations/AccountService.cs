using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.DTOs.Account;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Dictionary<string, bool> _permissionCache = new();
        public AccountService(SignInManager<User> signInManager, UserManager<User> userManager,
                         DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> LoginAsync(LoginDTO dto)
        {
            try
            {
                
                SignInResult result = await _signInManager.PasswordSignInAsync(
                    dto.Email, dto.Password, isPersistent: false, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                   
                    return Response.Failure("Correo o contraseña incorrectos.");
                }

                return Response.Success("Sesión iniciada.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Response<ProfileDTO>> GetProfileAsync()
        {
            try
            {
                User? user = await GetCurrentUserAsync();

                if (user is null)
                {
                    return Response<ProfileDTO>.Failure("No se encontró el usuario de la sesión.");
                }

                ProfileDTO dto = new ProfileDTO
                {
                    Email = user.Email ?? string.Empty,
                    RoleName = user.Role?.Name ?? string.Empty,
                    Document = user.Document,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                };

                return Response<ProfileDTO>.Success(dto);
            }
            catch (Exception ex)
            {
                return Response<ProfileDTO>.Failure(ex);
            }
        }

        public async Task<Response> UpdateProfileAsync(ProfileDTO dto)
        {
            try
            {
                User? user = await GetCurrentUserAsync();

                if (user is null)
                {
                    return Response.Failure("No se encontró el usuario de la sesión.");
                }

                bool documentTaken = await _context.Users
                    .AnyAsync(u => u.Document == dto.Document && u.Id != user.Id);

                if (documentTaken)
                {
                    return Response.Failure("Ya existe otro usuario con ese documento.");
                }

             
                user.Document = dto.Document;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.PhoneNumber = dto.PhoneNumber;

                await _context.SaveChangesAsync();

                return Response.Success("Perfil actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }

        public async Task<Response> ChangePasswordAsync(ChangePasswordDTO dto)
        {
            try
            {
                User? user = await GetCurrentUserAsync();

                if (user is null)
                {
                    return Response.Failure("No se encontró el usuario de la sesión.");
                }

                if (dto.CurrentPassword == dto.NewPassword)
                {
                    return Response.Failure("La nueva contraseña debe ser distinta a la actual.");
                }

                
                IdentityResult result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

                if (!result.Succeeded)
                {
                    if (result.Errors.Any(e => e.Code == "PasswordMismatch"))
                    {
                        return Response.Failure("La contraseña actual no es correcta.");
                    }

                    List<string> errors = result.Errors.Select(e => e.Description).ToList();
                    return Response.Failure("No se pudo cambiar la contraseña.", errors);
                }

                
                await _signInManager.RefreshSignInAsync(user);

                return Response.Success("Contraseña actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }
        public bool AccountUserIsAuthenticated()
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            return user?.Identity is not null && user.Identity.IsAuthenticated;
        }

        public async Task<bool> AccountUserIsAuthorizedAsync(string permissionCode)
        {
           
            if (_permissionCache.TryGetValue(permissionCode, out bool cached))
            {
                return cached;
            }

            string? userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
            {
                return false;
            }

            
            bool isAuthorized = await _context.Users.AnyAsync(u =>
                u.Id == userId &&
                (u.Role!.Name == Env.SUPER_ADMIN_ROLE_NAME ||
                 u.Role!.RolePermissions.Any(rp => rp.Permission!.Code == permissionCode)));

            
            _permissionCache[permissionCode] = isAuthorized;

            return isAuthorized;
        }

        public async Task<bool> AccountUserIsSuperAdminAsync()
        {
            string? userId = GetCurrentUserId();

            if (userId is null)
            {
                return false;
            }

            return await _context.Users.AnyAsync(u =>
                u.Id == userId && u.Role!.Name == Env.SUPER_ADMIN_ROLE_NAME);
        }

        public string? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
            {
                return null;
            }

            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}