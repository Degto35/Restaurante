using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
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

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<RoleDTO>> response = await _rolesService.GetPaginationAsync(request);

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
            Response<RoleDTO> response = await _rolesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result!);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateRoleDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            Response<CreateRoleDTO> response = await _rolesService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al crear el rol.");
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Response<RoleDTO> response = await _rolesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            UpdateRoleDTO dto = new UpdateRoleDTO
            {
                Id = response.Result!.Id,
                Name = response.Result.Name
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            Response<UpdateRoleDTO> response = await _rolesService.UpdateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al actualizar el rol.");
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            Response response = await _rolesService.DeleteAsync(id);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}