using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Roles;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IRolesService
    {
        Task<Response<PaginationResponse<RoleDTO>>> GetPaginationAsync(PaginationRequest request);
        Task<Response<RoleDTO>> GetOneAsync(Guid id);
        Task<Response<CreateRoleDTO>> CreateAsync(CreateRoleDTO dto);
        Task<Response<UpdateRoleDTO>> UpdateAsync(UpdateRoleDTO dto);
        Task<Response> DeleteAsync(Guid id);
    }
}