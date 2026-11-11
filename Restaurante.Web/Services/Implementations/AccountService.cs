using Microsoft.AspNetCore.Identity;
using Restaurante.Web.Core;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.DTOs.Account;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;

        public AccountService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
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
    }
}