using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.Models;

namespace Restaurante.Web.Controllers
{
    public class PlatoController : Controller
    {
        private readonly DataContext _context;

        public PlatoController(DataContext context)
        {
            _context = context;
        }

        // LISTAR PLATOS
        public async Task<IActionResult> Index()
        {
            var platos = await _context.Platos
                .Include(p => p.Categorias)
                .ToListAsync();

            return View(platos);
        }

        // FORMULARIO DE REGISTRO
        public async Task<IActionResult> Create()
        {
            await CargarCategorias();
            return View();
        }

        // GUARDAR PLATO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plato plato)
        {
            if (!await _context.Categorias.AnyAsync(
                c => c.Id_Categoria == plato.Id_Categoria))
            {
                ModelState.AddModelError(
                    "Id_Categoria", "Selecciona una categoría válida.");
            }

            if (ModelState.IsValid)
            {
                _context.Platos.Add(plato);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await CargarCategorias(plato.Id_Categoria);
            return View(plato);
        }

        // FORMULARIO DE EDICION
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var plato = await _context.Platos.FindAsync(id);

            if (plato == null)
                return NotFound();

            await CargarCategorias(plato.Id_Categoria);
            return View(plato);
        }

        // GUARDAR CAMBIOS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id_Plato,Nombre,Descripcion,Precio,Disponible,Id_Categoria")]
            Plato plato)
        {
            if (id != plato.Id_Plato)
                return NotFound();

            if (!await _context.Categorias.AnyAsync(
                c => c.Id_Categoria == plato.Id_Categoria))
            {
                ModelState.AddModelError(
                    "Id_Categoria", "Selecciona una categoría válida.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(plato);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await CargarCategorias(plato.Id_Categoria);
            return View(plato);
        }

        // CONFIRMAR ELIMINACION
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var plato = await _context.Platos
                .Include(p => p.Categorias)
                .Include(p => p.DetallesPedido)
                .FirstOrDefaultAsync(p => p.Id_Plato == id);

            if (plato == null)
                return NotFound();

            return View(plato);
        }

        // ELIMINAR PLATO
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plato = await _context.Platos
                .Include(p => p.DetallesPedido)
                .FirstOrDefaultAsync(p => p.Id_Plato == id);

            if (plato == null)
                return NotFound();

            if (plato.DetallesPedido.Any())
            {
                TempData["Error"] =
                    "No se puede eliminar este plato porque tiene pedidos asociados.";

                return RedirectToAction(nameof(Index));
            }

            _context.Platos.Remove(plato);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // CARGAR CATEGORIAS EN LOS FORMULARIOS
        private async Task CargarCategorias(int? seleccionada = null)
        {
            ViewBag.Categorias = new SelectList(
                await _context.Categorias.ToListAsync(),
                "Id_Categoria",
                "Nombre",
                seleccionada
            );
        }
    }
}
