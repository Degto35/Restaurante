using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.DTOs.Roles;
using Restaurante.Web.Services.Abstractions;


namespace Restaurante.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _rolesService.ObtenerTodosAsync();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View(new CrearRolDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearRolDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            if (await _rolesService.CrearAsync(dto))
            {
                TempData["Exito"] = "Rol creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al crear el rol.");
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var rol = await _rolesService.ObtenerPorIdAsync(id);
            if (rol == null) return NotFound();

            return View(new ActualizarRolDTO { IdRol = rol.IdRol, NombreRol = rol.NombreRol });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ActualizarRolDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            if (await _rolesService.ActualizarAsync(dto))
            {
                TempData["Exito"] = "Rol actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al actualizar el rol.");
            return View(dto);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var rol = await _rolesService.ObtenerPorIdAsync(id);
            if (rol == null) return NotFound();

            return View(rol);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await _rolesService.EliminarAsync(id))
                TempData["Exito"] = "Rol eliminado correctamente.";
            else
                TempData["Error"] = "No se puede eliminar el rol porque tiene empleados asociados.";

            return RedirectToAction(nameof(Index));
        }
    }
}