using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Categories;
using Restaurante.Web.Services.Abstractions;
using Restaurante.Web.Core.Attributes;


namespace Restaurante.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        [CustomAuthorize("categories.show")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<CategoryDTO>> response = await _categoriesService.GetPaginationAsync(request);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result!);
        }

        [HttpGet]
        [CustomAuthorize("categories.show")]
        public async Task<IActionResult> Details(Guid id)
        {
            Response<CategoryDTO> response = await _categoriesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result!);
        }

        [HttpGet]
        [CustomAuthorize("categories.create")]
        public IActionResult Create()
        {
            return View(new CreateCategoryDTO());
        }

        [HttpPost]
        [CustomAuthorize("categories.create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            Response<CreateCategoryDTO> response = await _categoriesService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al crear la categoría.");
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [CustomAuthorize("categories.update")]
        public async Task<IActionResult> Edit(Guid id)
        {
            Response<CategoryDTO> response = await _categoriesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            UpdateCategoryDTO dto = new UpdateCategoryDTO
            {
                Id = response.Result!.Id,
                Name = response.Result.Name,
                Description = response.Result.Description,
                IsActive = response.Result.IsActive
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize("categories.update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            Response<UpdateCategoryDTO> response = await _categoriesService.UpdateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al actualizar la categoría.");
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize("categories.delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            Response response = await _categoriesService.DeleteAsync(id);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}