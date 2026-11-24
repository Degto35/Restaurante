using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Attributes;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Orders;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers.Api
{
    [Route("api/orders")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersApiController : ApiController
    {
        private readonly IOrdersService _ordersService;

        public OrdersApiController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet]
        [CustomAuthorize("orders.show")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<OrderDTO>> response = await _ordersService.GetPaginationAsync(request);
            return ControllerBasicValidation(response);
        }

        [HttpGet("{id:guid}")]
        [CustomAuthorize("orders.show")]
        public async Task<IActionResult> GetOne([FromRoute] Guid id)
        {
            Response<OrderDTO> response = await _ordersService.GetOneAsync(id);
            return ControllerBasicValidation(response);
        }

        [HttpPost]
        [CustomAuthorize("orders.create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
        {
            Response<OrderDTO> response = await _ordersService.CreateAsync(dto);
            return ControllerBasicValidation(response, ModelState, response.IsSuccess ? StatusCodes.Status201Created : null);
        }

        [HttpPut("status")]
        [CustomAuthorize("orders.update")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderDTO dto)
        {
            Response response = await _ordersService.UpdateStatusAsync(dto);
            return ControllerBasicValidation(response, ModelState);
        }

        [HttpPost("details")]
        [CustomAuthorize("orders.update")]
        public async Task<IActionResult> AddDetail([FromBody] CreateOrderDetailDTO dto)
        {
            Response response = await _ordersService.AddDetailAsync(dto);
            return ControllerBasicValidation(response, ModelState);
        }

        [HttpDelete("details/{detailId:guid}")]
        [CustomAuthorize("orders.update")]
        public async Task<IActionResult> RemoveDetail([FromRoute] Guid detailId)
        {
            Response response = await _ordersService.RemoveDetailAsync(detailId);
            return ControllerBasicValidation(response);
        }

        [HttpPost("pay")]
        [CustomAuthorize("orders.pay")]
        public async Task<IActionResult> Pay([FromBody] PayOrderDTO dto)
        {
            Response response = await _ordersService.PayAsync(dto);
            return ControllerBasicValidation(response, ModelState);
        }
    }
}