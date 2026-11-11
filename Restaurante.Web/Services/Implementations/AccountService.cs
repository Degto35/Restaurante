using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.DTOs.Account;
using Restaurante.Web.Services.Abstractions;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.WebUtilities;

namespace Restaurante.Web.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly Dictionary<string, bool> _permissionCache = new();
        public AccountService(SignInManager<User> signInManager, UserManager<User> userManager,
                        DataContext context, IHttpContextAccessor httpContextAccessor,
                        IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
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

        public async Task<Response> SendPasswordResetEmailAsync(ForgotPasswordDTO dto, Func<string, string> buildResetLink)
        {
            try
            {
                User? user = await _userManager.FindByEmailAsync(dto.Email);

                
                if (user is null)
                {
                    return Response.Success();
                }

                string token = await _userManager.GeneratePasswordResetTokenAsync(user);

               
                string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                string link = HtmlEncoder.Default.Encode(buildResetLink(encodedToken));
                string name = HtmlEncoder.Default.Encode(user.FirstName);

                string body =
                    $"<p>Hola {name},</p>" +
                    "<p>Recibimos una solicitud para restablecer la contraseña de tu cuenta en Sabor &amp; Tradición.</p>" +
                    $"<p><a href=\"{link}\">Restablecer mi contraseña</a></p>" +
                    "<p>El enlace vence en 1 hora y solo se puede usar una vez. " +
                    "Si no lo solicitaste, ignora este correo: tu contraseña no cambiará.</p>";

                await _emailService.SendAsync(user.Email!, "Restablece tu contraseña", body);

                return Response.Success();
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }

        public async Task<Response> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            const string invalidLink = "El enlace no es válido o ya venció. Solicita uno nuevo.";

            try
            {
                User? user = await _userManager.FindByEmailAsync(dto.Email);

                if (user is null)
                {
                    return Response.Failure(invalidLink);
                }

                string token;

                try
                {
                    token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
                }
                catch (FormatException)
                {
                    return Response.Failure(invalidLink);
                }

                IdentityResult result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

                if (!result.Succeeded)
                {
                    
                    if (result.Errors.Any(e => e.Code == "InvalidToken"))
                    {
                        return Response.Failure(invalidLink);
                    }

                    List<string> errors = result.Errors.Select(e => e.Description).ToList();
                    return Response.Failure("No se pudo restablecer la contraseña.", errors);
                }

                return Response.Success("Contraseña restablecida. Ya puedes iniciar sesión.");
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