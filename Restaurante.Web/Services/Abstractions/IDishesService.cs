using Restaurante.Web.DTOs.Dishes;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IDishesService
    {
        Task<List<DishDTO>> ObtenerTodosAsync();
        Task<DishDTO?> ObtenerPorIdAsync(Guid id);
        Task<bool> CrearAsync(CreateDishDTO dto);
        Task<bool> ActualizarAsync(UpdateDishDTO dto);
        Task<bool> EliminarAsync(Guid id);
    }
}
