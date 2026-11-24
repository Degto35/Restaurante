using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Users;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IUsersService
    {
        Task<Response<PaginationResponse<UserDTO>>> GetPaginationAsync(PaginationRequest request);
        Task<Response<UserDTO>> GetOneAsync(string id);
        Task<Response> CreateAsync(CreateUserDTO dto);
        Task<Response> UpdateAsync(UpdateUserDTO dto);
        Task<Response> DeleteAsync(string id);
    }
}