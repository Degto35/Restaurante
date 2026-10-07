using Restaurante.Web.DTOs.Platos;

namespace Restaurante.Web.Services
{
    public interface IPlatosService
    {
        Task<List<PlatoDTO>> ObtenerTodosAsync();
        Task<PlatoDTO?> ObtenerPorIdAsync(Guid id);
        Task<bool> CrearAsync(CrearPlatoDTO dto);
        Task<bool> ActualizarAsync(ActualizarPlatoDTO dto);
        Task<bool> EliminarAsync(Guid id);
    }
}
