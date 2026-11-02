using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.DTOs.Categories;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this._categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.ObtenerTodasAsync();
            return View(categories);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var category = await _categoriesService.ObtenerPorIdAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _categoriesService.CrearAsync(dto);
                if (resultado)
                {
                    TempData["Exito"] = "Categoría creada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al crear la categoría.");
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categoriesService.ObtenerPorIdAsync(id);
            if (category == null) return NotFound();
            var dto = new UpdateCategoryDTO
            {
                IdCategories = category.IdCategories,
                Name = category.Name,
                Description = category.Description,
                State = category.State
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _categoriesService.ActualizarAsync(dto);
                if (resultado)
                {
                    TempData["Exito"] = "Categoría actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al actualizar la categoría.");
            }
            return View(dto);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _categoriesService.ObtenerPorIdAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var resultado = await _categoriesService.EliminarAsync(id);
            if (resultado)
            {
                TempData["Exito"] = "Categoría eliminada correctamente.";
            }
            else
            {
                TempData["Error"] = "No se puede eliminar la categoría porque tiene platos asociados.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}