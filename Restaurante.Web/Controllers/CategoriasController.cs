using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.DTOs.Categorias;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ICategoriasService _categoriaService;

        public CategoriasController(ICategoriasService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.ObtenerTodasAsync();
            return View(categorias);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var categoria = await _categoriaService.ObtenerPorIdAsync(id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearCategoriaDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _categoriaService.CrearAsync(dto);
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
            var categoria = await _categoriaService.ObtenerPorIdAsync(id);
            if (categoria == null) return NotFound();
            var dto = new ActualizarCategoriaDTO
            {
                IdCategorias = categoria.IdCategorias,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Estado = categoria.Estado
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ActualizarCategoriaDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _categoriaService.ActualizarAsync(dto);
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
            var categoria = await _categoriaService.ObtenerPorIdAsync(id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var resultado = await _categoriaService.EliminarAsync(id);
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