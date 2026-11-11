using Restaurante.Web.Core;
using Restaurante.Web.DTOs.Account;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IAccountService
    {
        Task<Response> LoginAsync(LoginDTO dto);
        Task LogoutAsync();
    }
}