using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.DTOs.Tables;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers
{
    public class TablesController : Controller
    {
        private readonly ITablesService _tablesService;

        public TablesController(ITablesService tablesService)
        {
            this._tablesService = tablesService;
        }

        public async Task<IActionResult> Index()
        {
            var tables = await _tablesService.ObtenerTodasAsync();
            return View(tables);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var table = await _tablesService.ObtenerPorIdAsync(id);
            if (table == null) return NotFound();

            return View(table);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTableDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _tablesService.CrearAsync(dto);
                if (resultado)
                {
                    TempData["Exito"] = "Mesa creada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al crear la mesa.");
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var table = await _tablesService.ObtenerPorIdAsync(id);
            if (table == null) return NotFound();

            var dto = new UpdateTableDTO
            {
                IdTable = table.IdTable,
                Number = table.Number,
                Capacity = table.Capacity,
                IsAvailable = table.IsAvailable
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateTableDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _tablesService.ActualizarAsync(dto);
                if (resultado)
                {
                    TempData["Exito"] = "Mesa actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al actualizar la mesa.");
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var table = await _tablesService.ObtenerPorIdAsync(id);
            if (table == null) return NotFound();

            return View(table);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var resultado = await _tablesService.EliminarAsync(id);
            if (resultado)
            {
                TempData["Exito"] = "Mesa eliminada correctamente.";
            }
            else
            {
                TempData["Error"] = "No se puede eliminar la mesa porque tiene pedidos asociados.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
