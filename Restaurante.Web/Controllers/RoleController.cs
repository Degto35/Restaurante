using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.DTOs.Role;
using Restaurante.Web.Services.Abstractions;


namespace Restaurante.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            var role = await _roleService.ObtenerTodosAsync();
            return View(role);
        }

        public IActionResult Create()
        {
            return View(new CreateRoleDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            if (await _roleService.CrearAsync(dto))
            {
                TempData["Exito"] = "Rol creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al crear el rol.");
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var role = await _roleService.ObtenerPorIdAsync(id);
            if (role == null) return NotFound();

            return View(new UpdateRoleDTO { IdRole = role.IdRole, NameRole = role.NameRole });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateRoleDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            if (await _roleService.ActualizarAsync(dto))
            {
                TempData["Exito"] = "Rol actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al actualizar el rol.");
            return View(dto);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _roleService.ObtenerPorIdAsync(id);
            if (role == null) return NotFound();

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await _roleService.EliminarAsync(id))
                TempData["Exito"] = "Rol eliminado correctamente.";
            else
                TempData["Error"] = "No se puede eliminar el rol porque tiene empleados asociados.";

            return RedirectToAction(nameof(Index));
        }
    }
}