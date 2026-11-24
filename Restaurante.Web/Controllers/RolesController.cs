using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Attributes;
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
        [CustomAuthorize("roles.show")]
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
        [CustomAuthorize("roles.show")]
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
        [CustomAuthorize("roles.create")]
        public async Task<IActionResult> Create()
        {
            CreateRoleDTO dto = new CreateRoleDTO
            {
                Permissions = await LoadPermissionsAsync([])
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize("roles.create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                dto.Permissions = await LoadPermissionsAsync(dto.PermissionIds);
                return View(dto);
            }

            Response<CreateRoleDTO> response = await _rolesService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al crear el rol.");
                dto.Permissions = await LoadPermissionsAsync(dto.PermissionIds);
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [CustomAuthorize("roles.update")]
        public async Task<IActionResult> Edit(Guid id)
        {
            Response<RoleDTO> response = await _rolesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

          
            if (response.Result!.Name == Env.SUPER_ADMIN_ROLE_NAME)
            {
                TempData["Error"] = "El rol de administrador no se puede modificar.";
                return RedirectToAction(nameof(Index));
            }

            UpdateRoleDTO dto = new UpdateRoleDTO
            {
                Id = response.Result.Id,
                Name = response.Result.Name,
                Permissions = response.Result.Permissions
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize("roles.update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                dto.Permissions = await LoadPermissionsAsync(dto.PermissionIds);
                return View(dto);
            }

            Response<UpdateRoleDTO> response = await _rolesService.UpdateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al actualizar el rol.");
                dto.Permissions = await LoadPermissionsAsync(dto.PermissionIds);
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize("roles.delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            Response response = await _rolesService.DeleteAsync(id);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

       
        private async Task<List<PermissionForRoleDTO>> LoadPermissionsAsync(IEnumerable<Guid> selectedIds)
        {
            Response<List<PermissionForRoleDTO>> response = await _rolesService.GetPermissionsAsync(selectedIds);

            return response.IsSuccess ? response.Result! : [];
        }
    }
}