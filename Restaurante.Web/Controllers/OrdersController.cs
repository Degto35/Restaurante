using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurante.Web.Core;
using Restaurante.Web.DTOs.DiningTables;
using Restaurante.Web.DTOs.Orders;
using Restaurante.Web.Services.Abstractions;
using Restaurante.Web.Services.Implementations;

namespace Restaurante.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly IDiningTablesService _diningTablesService;
        private readonly IEmployeesService _employeesService;
        private readonly IDishesService _dishesService;

        public OrdersController(
            IOrdersService ordersService,
            IDiningTablesService diningTablesService,
            IEmployeesService employeesService,
            IDishesService dishesService)
        {
            _ordersService = ordersService;
            _diningTablesService = diningTablesService;
            _employeesService = employeesService;
            _dishesService = dishesService;
        }

        public async Task<IActionResult> Index()
        {
            Response<List<OrderDTO>> response = await _ordersService.GetAllAsync();

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return View(new List<OrderDTO>());
            }

            return View(response.Result!);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            Response<OrderDTO> response = await _ordersService.GetByIdAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            await LoadDishesAsync();
            return View(response.Result!);
        }

        public async Task<IActionResult> Create()
        {
            await LoadCreateListsAsync();
            return View(new CreateOrderDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCreateListsAsync();
                return View(dto);
            }

            Response<OrderDTO> response = await _ordersService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al crear el pedido.");
                await LoadCreateListsAsync();
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

        private async Task LoadCreateListsAsync()
        {
            var tablesResponse = await _diningTablesService.GetAllAsync();
            var employees = await _employeesService.ObtenerTodosAsync();

            ViewBag.Tables = (tablesResponse.Result ?? new List<DiningTableDTO>())
                .Where(t => t.IsAvailable)
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"Mesa {t.Number} ({t.Capacity} personas)"
                })
                .ToList();

            ViewBag.Employees = employees
                .Select(e => new SelectListItem
                {
                    Value = e.IdEmployee.ToString(),
                    Text = e.Name
                })
                .ToList();
        }


        private async Task LoadDishesAsync()
        {
            var dishes = await _dishesService.ObtenerTodosAsync();

            ViewBag.Dishes = dishes
                .Where(d => d.State)
                .Select(d => new SelectListItem
                {
                    Value = d.IdDish.ToString(),
                    Text = $"{d.Name} - {d.Price:C}"
                })
                .ToList();
        }
    }
}