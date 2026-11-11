using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.Models;

namespace Restaurante.Web.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly DataContext _context;

        public CategoriasController(DataContext context)
        {
            _context = context;
        }

        // Listar categorias
        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return View(categorias);
        }

        // Mostrar formulario
        public IActionResult Create()
        {
            return View();
        }

        // Registrar categoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categorias categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }
    }
}
