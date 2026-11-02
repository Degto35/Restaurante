using Restaurante.Web.DTOs.Role;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IRoleService
    {
        Task<List<RoleDTO>> ObtenerTodosAsync();
        Task<RoleDTO?> ObtenerPorIdAsync(Guid id);
        Task<bool> CrearAsync(CreateRoleDTO dto);
        Task<bool> ActualizarAsync(UpdateRoleDTO dto);
        Task<bool> EliminarAsync(Guid id);
    }
}
