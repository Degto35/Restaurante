using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Categories;

namespace Restaurante.Web.Services.Abstractions
{
    public interface ICategoriesService
    {
        Task<Response<PaginationResponse<CategoryDTO>>> GetPaginationAsync(PaginationRequest request);
        Task<Response<List<CategoryDTO>>> GetAllAsync();
        Task<Response<CategoryDTO>> GetOneAsync(Guid id);
        Task<Response<CreateCategoryDTO>> CreateAsync(CreateCategoryDTO dto);
        Task<Response<UpdateCategoryDTO>> UpdateAsync(UpdateCategoryDTO dto);
        Task<Response> DeleteAsync(Guid id);
    }
}