using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.DTOs.Employees;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService _EmployeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _EmployeesService = employeesService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _EmployeesService.ObtenerTodosAsync();
            return View(employees);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var employee = await _EmployeesService.ObtenerPorIdAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        public async Task<IActionResult> Create()
        {
            var dto = new CreateEmployeeDTO
            {
                Roles = await _EmployeesService.ObtenerRolesAsync()
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeDTO dto)
        {
            if (ModelState.IsValid)
            {
                if (await _EmployeesService.CrearAsync(dto))
                {
                    TempData["Exito"] = "Empleado creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al crear el empleado.");
            }

            dto.Roles = await _EmployeesService.ObtenerRolesAsync();
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var employee = await _EmployeesService.ObtenerPorIdAsync(id);
            if (employee == null) return NotFound();

            var dto = new UpdateEmployeeDTO
            {
                IdEmployee = employee.IdEmployee,
                Name = employee.Name,
                Telephone = employee.Telephone,
                Email = employee.Email,
                IdRole = employee.IdRole,
                Role = await _EmployeesService.ObtenerRolesAsync()
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateEmployeeDTO dto)
        {
            if (ModelState.IsValid)
            {
                if (await _EmployeesService.ActualizarAsync(dto))
                {
                    TempData["Exito"] = "Empleado actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error al actualizar el empleado.");
            }

            dto.Role = await _EmployeesService.ObtenerRolesAsync();
            return View(dto);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var empleado = await _EmployeesService.ObtenerPorIdAsync(id);
            if (empleado == null) return NotFound();

            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await _EmployeesService.EliminarAsync(id))
                TempData["Exito"] = "Empleado eliminado correctamente.";
            else
                TempData["Error"] = "No se pudo eliminar el empleado.";

            return RedirectToAction(nameof(Index));
        }
    }
}