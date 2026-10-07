using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurante.Web.DTOs.Platos;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers
{
    public class PlatosController : Controller
    {
        private readonly IPlatosService _platoService;
        private readonly ICategoriasService _categoriaService; 

        public PlatosController(IPlatosService platoService, ICategoriasService categoriaService)
        {
            _platoService = platoService;
            _categoriaService = categoriaService;
        }
        public async Task<IActionResult> Index()
        {
            var platos = await _platoService.ObtenerTodosAsync();
            return View(platos);
        }
        public async Task<IActionResult> Create()
        {
            await CargarCategoriasEnViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearPlatoDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _platoService.CrearAsync(dto);
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
            var plato = await _platoService.ObtenerPorIdAsync(id);
            if (plato == null) return NotFound();

            var dto = new ActualizarPlatoDTO
            {
                IdPlato = plato.IdPlato,
                Nombre = plato.Nombre,
                Descripcion = plato.Descripcion,
                Precio = plato.Precio,
                IdCategorias = plato.IdCategorias,
                Estado = plato.Estado
            };

            await CargarCategoriasEnViewBag(dto.IdCategorias);
            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ActualizarPlatoDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _platoService.ActualizarAsync(dto);
                if (resultado)
                {
                    TempData["Exito"] = "Plato actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al actualizar el plato.");
            }

            await CargarCategoriasEnViewBag(dto.IdCategorias);
            return View(dto);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var plato = await _platoService.ObtenerPorIdAsync(id);
            if (plato == null) return NotFound();

            return View(plato);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var resultado = await _platoService.EliminarAsync(id);
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
            var categorias = await _categoriaService.ObtenerTodasAsync();
            ViewBag.Categorias = new SelectList(categorias, "IdCategorias", "Nombre", idCategoriaSeleccionada);
        }
    }
}