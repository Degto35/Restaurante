using Restaurante.Web.DTOs.Categorias;

namespace Restaurante.Web.Services
{
    public interface ICategoriasService
    {
        Task<List<CategoriaDTO>> ObtenerTodasAsync();
        Task<CategoriaDTO?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(CrearCategoriaDTO dto);
        Task<bool> ActualizarAsync(ActualizarCategoriaDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
