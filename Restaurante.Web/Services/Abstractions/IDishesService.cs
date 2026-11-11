using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Dishes;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IDishesService
    {
        Task<Response<PaginationResponse<DishDTO>>> GetPaginationAsync(PaginationRequest request);
        Task<Response<DishDTO>> GetOneAsync(Guid id);
        Task<Response<CreateDishDTO>> CreateAsync(CreateDishDTO dto);
        Task<Response<UpdateDishDTO>> UpdateAsync(UpdateDishDTO dto);
        Task<Response> DeleteAsync(Guid id);
    }
}
