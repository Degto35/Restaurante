using Restaurante.Web.DTOs.Categories;

namespace Restaurante.Web.Services.Abstractions
{
    public interface ICategoriesService
    {
        Task<List<CategoryDTO>> ObtenerTodasAsync();
        Task<CategoryDTO?> ObtenerPorIdAsync(Guid id);
        Task<bool> CrearAsync(CreateCategoryDTO dto);
        Task<bool> ActualizarAsync(UpdateCategoryDTO dto);
        Task<bool> EliminarAsync(Guid id);
    }
}
