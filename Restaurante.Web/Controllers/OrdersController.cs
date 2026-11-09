using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurante.Web.Core;
using Restaurante.Web.DTOs.DiningTables;
using Restaurante.Web.DTOs.Orders;
using Restaurante.Web.Services.Abstractions;
using Restaurante.Web.Services.Implementations;
using Restaurante.Web.Helpers.Abstractions;
using Restaurante.Web.Core.Pagination;

namespace Restaurante.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly ISelectListHelper _selectListHelper;

        public OrdersController(IOrdersService ordersService, ISelectListHelper selectListHelper)
        {
            _ordersService = ordersService;
            _selectListHelper = selectListHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<OrderDTO>> response = await _ordersService.GetPaginationAsync(request);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result!);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            Response<OrderDTO> response = await _ordersService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            OrderDTO order = response.Result!;
            order.Dishes = await _selectListHelper.GetAvailableDishesSelectListAsync();
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateOrderDTO dto = new CreateOrderDTO();
            await FillListsAsync(dto);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await FillListsAsync(dto);
                return View(dto);
            }

            Response<OrderDTO> response = await _ordersService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al crear el pedido.");
                await FillListsAsync(dto);
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Details), new { id = response.Result!.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetail(CreateOrderDetailDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Seleccione un plato y una cantidad válida.";
                return RedirectToAction(nameof(Details), new { id = dto.OrderId });
            }

            Response response = await _ordersService.AddDetailAsync(dto);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Details), new { id = dto.OrderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveDetail(Guid detailId, Guid orderId)
        {
            Response response = await _ordersService.RemoveDetailAsync(detailId);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Details), new { id = orderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateOrderDTO dto)
        {
            Response response = await _ordersService.UpdateStatusAsync(dto);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Details), new { id = dto.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(PayOrderDTO dto)
        {
            Response response = await _ordersService.PayAsync(dto);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Details), new { id = dto.OrderId });
        }

        private async Task FillListsAsync(CreateOrderDTO dto)
        {
            dto.Tables = await _selectListHelper.GetAvailableTablesSelectListAsync();
            dto.Employees = await _selectListHelper.GetEmployeesSelectListAsync();
        }
    }
}