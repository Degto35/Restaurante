using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Orders;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IOrdersService
    {
        Task<Response<PaginationResponse<OrderDTO>>> GetPaginationAsync(PaginationRequest request);
        Task<Response<OrderDTO>> GetOneAsync(Guid id);
        Task<Response<OrderDTO>> CreateAsync(CreateOrderDTO dto);
        Task<Response> UpdateStatusAsync(UpdateOrderDTO dto);
        Task<Response> AddDetailAsync(CreateOrderDetailDTO dto);
        Task<Response> RemoveDetailAsync(Guid detailId);
        Task<Response> PayAsync(PayOrderDTO dto);
    }
}