using Restaurante.Web.DTOs.Roles;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IRolesService
    {
        Task<List<RolDTO>> ObtenerTodosAsync();
        Task<RolDTO?> ObtenerPorIdAsync(Guid id);
        Task<bool> CrearAsync(CrearRolDTO dto);
        Task<bool> ActualizarAsync(ActualizarRolDTO dto);
        Task<bool> EliminarAsync(Guid id);
    }
}
