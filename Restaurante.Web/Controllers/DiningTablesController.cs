using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.DiningTables;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers
{
    public class DiningTablesController : Controller
    {
        private readonly IDiningTablesService _diningTablesService;

        public DiningTablesController(IDiningTablesService diningTablesService)
        {
            _diningTablesService = diningTablesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<DiningTableDTO>> response = await _diningTablesService.GetPaginationAsync(request);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result!);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Response<DiningTableDTO> response = await _diningTablesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result!);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateDiningTableDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDiningTableDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            Response<CreateDiningTableDTO> response = await _diningTablesService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al crear la mesa.");
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Response<DiningTableDTO> response = await _diningTablesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            UpdateDiningTableDTO dto = new UpdateDiningTableDTO
            {
                Id = response.Result!.Id,
                Number = response.Result.Number,
                Capacity = response.Result.Capacity
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateDiningTableDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            Response<UpdateDiningTableDTO> response = await _diningTablesService.UpdateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al actualizar la mesa.");
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            Response response = await _diningTablesService.DeleteAsync(id);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAvailability(Guid id)
        {
            Response response = await _diningTablesService.ToggleAvailabilityAsync(id);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}