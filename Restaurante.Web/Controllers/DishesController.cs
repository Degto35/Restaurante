using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurante.Web.DTOs.Dishes;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers
{
    public class DishesController : Controller
    {
        private readonly IDishesService _DishesService;
        private readonly ICategoriesService _CategoriesService;

        public DishesController(IDishesService dishesService, ICategoriesService categoriesService)
        {
            _DishesService = dishesService;
            _CategoriesService = categoriesService;
        }
        public async Task<IActionResult> Index()
        {
            var dishes = await _DishesService.ObtenerTodosAsync();
            return View(dishes);
        }
        public async Task<IActionResult> Create()
        {
            await CargarCategoriasEnViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDishDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _DishesService.CrearAsync(dto);
                if (resultado)
                {
                    TempData["Exito"] = "Plato creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al crear el plato.");
            }
            await CargarCategoriasEnViewBag();
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var dish = await _DishesService.ObtenerPorIdAsync(id);
            if (dish == null) return NotFound();

            var dto = new UpdateDishDTO
            {
                IdDish = dish.IdDish,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                IdCategories = dish.IdCategories,
                State = dish.State
            };

            await CargarCategoriasEnViewBag(dto.IdCategories);
            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateDishDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _DishesService.ActualizarAsync(dto);
                if (resultado)
                {
                    TempData["Exito"] = "Plato actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al actualizar el plato.");
            }

            await CargarCategoriasEnViewBag(dto.IdCategories);
            return View(dto);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var dish = await _DishesService.ObtenerPorIdAsync(id);
            if (dish == null) return NotFound();

            return View(dish);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var resultado = await _DishesService.EliminarAsync(id);
            if (resultado)
            {
                TempData["Exito"] = "Plato eliminado correctamente.";
            }
            else
            {
                TempData["Error"] = "No se puede eliminar el plato porque está asociado a un pedido.";
            }
            return RedirectToAction(nameof(Index));
        }
        private async Task CargarCategoriasEnViewBag(Guid
            ? idCategoriaSeleccionada = null)
        {
            var categories = await _CategoriesService.ObtenerTodasAsync();
            ViewBag.Categorias = new SelectList(categories, "IdCategories", "Name", idCategoriaSeleccionada);
        }
    }
}