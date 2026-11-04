using Restaurante.Web.Core;
using Restaurante.Web.DTOs.Orders;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IOrdersService
    {
        Task<Response<List<OrderDTO>>> GetAllAsync();
        Task<Response<OrderDTO>> GetByIdAsync(Guid id);
        Task<Response<OrderDTO>> CreateAsync(CreateOrderDTO dto);
        Task<Response> UpdateStatusAsync(UpdateOrderDTO dto);
        Task<Response> AddDetailAsync(CreateOrderDetailDTO dto);
        Task<Response> RemoveDetailAsync(Guid detailId);
        Task<Response> PayAsync(PayOrderDTO dto);
    }
}