using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Dishes;
using Restaurante.Web.Helpers.Abstractions;
using Restaurante.Web.Services.Abstractions;
using Restaurante.Web.Core.Attributes;

namespace Restaurante.Web.Controllers
{
    public class DishesController : Controller
    {
        private readonly IDishesService _dishesService;
        private readonly ISelectListHelper _selectListHelper;

        public DishesController(IDishesService dishesService, ISelectListHelper selectListHelper)
        {
            _dishesService = dishesService;
            _selectListHelper = selectListHelper;
        }

        [HttpGet]
        [CustomAuthorize("dishes.show")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<DishDTO>> response = await _dishesService.GetPaginationAsync(request);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result!);
        }

        [HttpGet]
        [CustomAuthorize("dishes.show")]
        public async Task<IActionResult> Details(Guid id)
        {
            Response<DishDTO> response = await _dishesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result!);
        }

        [HttpGet]
        [CustomAuthorize("dishes.create")]
        public async Task<IActionResult> Create()
        {
            CreateDishDTO dto = new CreateDishDTO
            {
                Categories = await _selectListHelper.GetCategoriesSelectListAsync()
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize("dishes.create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDishDTO dto)
        {
            if (!ModelState.IsValid)
            {
                dto.Categories = await _selectListHelper.GetCategoriesSelectListAsync();
                return View(dto);
            }

            Response<CreateDishDTO> response = await _dishesService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al crear el plato.");
                dto.Categories = await _selectListHelper.GetCategoriesSelectListAsync();
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [CustomAuthorize("dishes.update")]
        public async Task<IActionResult> Edit(Guid id)
        {
            Response<DishDTO> response = await _dishesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            UpdateDishDTO dto = new UpdateDishDTO
            {
                Id = response.Result!.Id,
                Name = response.Result.Name,
                Description = response.Result.Description,
                Price = response.Result.Price,
                CategoryId = response.Result.CategoryId,
                IsActive = response.Result.IsActive,
                Categories = await _selectListHelper.GetCategoriesSelectListAsync(response.Result.CategoryId)
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize("dishes.update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateDishDTO dto)
        {
            if (!ModelState.IsValid)
            {
                dto.Categories = await _selectListHelper.GetCategoriesSelectListAsync(dto.CategoryId);
                return View(dto);
            }

            Response<UpdateDishDTO> response = await _dishesService.UpdateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al actualizar el plato.");
                dto.Categories = await _selectListHelper.GetCategoriesSelectListAsync(dto.CategoryId);
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize("dishes.delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            Response response = await _dishesService.DeleteAsync(id);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}