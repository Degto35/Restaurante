using Restaurante.Web.Core;
using Restaurante.Web.DTOs.Account;


namespace Restaurante.Web.Services.Abstractions
{
    public interface IAccountService
    {
        Task<Response> LoginAsync(LoginDTO dto);
        Task LogoutAsync();
        Task<Response<ProfileDTO>> GetProfileAsync();
        Task<Response> UpdateProfileAsync(ProfileDTO dto);
        bool AccountUserIsAuthenticated();
        Task<bool> AccountUserIsAuthorizedAsync(string permissionCode);
        Task<bool> AccountUserIsSuperAdminAsync();
        string? GetCurrentUserId();
    }
}