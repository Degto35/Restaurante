using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Employees;
using Restaurante.Web.Helpers.Abstractions;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService _employeesService;
        private readonly ISelectListHelper _selectListHelper;

        public EmployeesController(IEmployeesService employeesService, ISelectListHelper selectListHelper)
        {
            _employeesService = employeesService;
            _selectListHelper = selectListHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<EmployeeDTO>> response = await _employeesService.GetPaginationAsync(request);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result!);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Response<EmployeeDTO> response = await _employeesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result!);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateEmployeeDTO dto = new CreateEmployeeDTO
            {
                Roles = await _selectListHelper.GetRolesSelectListAsync()
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                dto.Roles = await _selectListHelper.GetRolesSelectListAsync();
                return View(dto);
            }

            Response<CreateEmployeeDTO> response = await _employeesService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al crear el empleado.");
                dto.Roles = await _selectListHelper.GetRolesSelectListAsync();
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Response<EmployeeDTO> response = await _employeesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            UpdateEmployeeDTO dto = new UpdateEmployeeDTO
            {
                Id = response.Result!.Id,
                Name = response.Result.Name,
                Telephone = response.Result.Telephone,
                Email = response.Result.Email,
                RoleId = response.Result.RoleId,
                Roles = await _selectListHelper.GetRolesSelectListAsync()
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateEmployeeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                dto.Roles = await _selectListHelper.GetRolesSelectListAsync();
                return View(dto);
            }

            Response<UpdateEmployeeDTO> response = await _employeesService.UpdateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al actualizar el empleado.");
                dto.Roles = await _selectListHelper.GetRolesSelectListAsync();
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            Response response = await _employeesService.DeleteAsync(id);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}