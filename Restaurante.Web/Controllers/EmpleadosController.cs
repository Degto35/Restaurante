using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.DTOs.Empleados;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IEmpleadosService _empleadosService;

        public EmpleadosController(IEmpleadosService empleadosService)
        {
            _empleadosService = empleadosService;
        }

        public async Task<IActionResult> Index()
        {
            var empleados = await _empleadosService.ObtenerTodosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var empleado = await _empleadosService.ObtenerPorIdAsync(id);
            if (empleado == null) return NotFound();

            return View(empleado);
        }

        public async Task<IActionResult> Create()
        {
            var dto = new CrearEmpleadoDTO
            {
                Roles = await _empleadosService.ObtenerRolesAsync()
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearEmpleadoDTO dto)
        {
            if (ModelState.IsValid)
            {
                if (await _empleadosService.CrearAsync(dto))
                {
                    TempData["Exito"] = "Empleado creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al crear el empleado.");
            }

            dto.Roles = await _empleadosService.ObtenerRolesAsync();
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var empleado = await _empleadosService.ObtenerPorIdAsync(id);
            if (empleado == null) return NotFound();

            var dto = new ActualizarEmpleadoDTO
            {
                IdEmpleado = empleado.IdEmpleado,
                Nombre = empleado.Nombre,
                Telefono = empleado.Telefono,
                Email = empleado.Email,
                IdRol = empleado.IdRol,
                Roles = await _empleadosService.ObtenerRolesAsync()
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ActualizarEmpleadoDTO dto)
        {
            if (ModelState.IsValid)
            {
                if (await _empleadosService.ActualizarAsync(dto))
                {
                    TempData["Exito"] = "Empleado actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al actualizar el empleado.");
            }

            dto.Roles = await _empleadosService.ObtenerRolesAsync();
            return View(dto);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var empleado = await _empleadosService.ObtenerPorIdAsync(id);
            if (empleado == null) return NotFound();

            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await _empleadosService.EliminarAsync(id))
                TempData["Exito"] = "Empleado eliminado correctamente.";
            else
                TempData["Error"] = "No se pudo eliminar el empleado.";

            return RedirectToAction(nameof(Index));
        }
    }
}