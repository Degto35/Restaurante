using Restaurante.Web.DTOs.Tables;

namespace Restaurante.Web.Services.Abstractions
{
    public interface ITablesService
    {
        Task<IEnumerable<TableDTO>> ObtenerTodasAsync();
        Task<TableDTO?> ObtenerPorIdAsync(Guid id);
        Task<bool> CrearAsync(CreateTableDTO dto);
        Task<bool> ActualizarAsync(UpdateTableDTO dto);
        Task<bool> EliminarAsync(Guid id);
        Task<bool> CambiarDisponibilidadAsync(Guid id);
    }
}
