using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.DiningTables;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IDiningTablesService
    {
        Task<Response<PaginationResponse<DiningTableDTO>>> GetPaginationAsync(PaginationRequest request);
        Task<Response<List<DiningTableDTO>>> GetAllAsync();
        Task<Response<DiningTableDTO>> GetOneAsync(Guid id);
        Task<Response<CreateDiningTableDTO>> CreateAsync(CreateDiningTableDTO dto);
        Task<Response<UpdateDiningTableDTO>> UpdateAsync(UpdateDiningTableDTO dto);
        Task<Response> DeleteAsync(Guid id);
        Task<Response> ToggleAvailabilityAsync(Guid id);
    }
}