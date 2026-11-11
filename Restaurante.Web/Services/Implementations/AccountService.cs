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
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(SignInManager<User> signInManager, DataContext context,
                              IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
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