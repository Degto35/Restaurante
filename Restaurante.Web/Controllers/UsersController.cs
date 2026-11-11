using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Attributes;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Users;
using Restaurante.Web.Helpers.Abstractions;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly ISelectListHelper _selectListHelper;

        public UsersController(IUsersService usersService, ISelectListHelper selectListHelper)
        {
            _usersService = usersService;
            _selectListHelper = selectListHelper;
        }

        [HttpGet]
        [CustomAuthorize("users.show")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<UserDTO>> response = await _usersService.GetPaginationAsync(request);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result!);
        }

        [HttpGet]
        [CustomAuthorize("users.show")]
        public async Task<IActionResult> Details(string id)
        {
            Response<UserDTO> response = await _usersService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result!);
        }

        [HttpGet]
        [CustomAuthorize("users.create")]
        public async Task<IActionResult> Create()
        {
            CreateUserDTO dto = new CreateUserDTO
            {
                Roles = await _selectListHelper.GetRolesSelectListAsync()
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize("users.create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                dto.Roles = await _selectListHelper.GetRolesSelectListAsync();
                return View(dto);
            }

            Response response = await _usersService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al crear el usuario.");

                if (response.Errors is not null)
                {
                    foreach (string error in response.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }

                dto.Roles = await _selectListHelper.GetRolesSelectListAsync();
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [CustomAuthorize("users.update")]
        public async Task<IActionResult> Edit(string id)
        {
            Response<UserDTO> response = await _usersService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            UpdateUserDTO dto = new UpdateUserDTO
            {
                Id = response.Result!.Id,
                Email = response.Result.Email,
                FirstName = response.Result.FirstName,
                LastName = response.Result.LastName,
                Document = response.Result.Document,
                PhoneNumber = response.Result.PhoneNumber,
                RoleId = response.Result.RoleId,
                Roles = await _selectListHelper.GetRolesSelectListAsync()
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize("users.update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await FillEditListsAsync(dto);
                return View(dto);
            }

            Response response = await _usersService.UpdateAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message ?? "Error al actualizar el usuario.");

                if (response.Errors is not null)
                {
                    foreach (string error in response.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }

                await FillEditListsAsync(dto);
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize("users.delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            Response response = await _usersService.DeleteAsync(id);

            TempData[response.IsSuccess ? "Exito" : "Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        private async Task FillEditListsAsync(UpdateUserDTO dto)
        {
            dto.Roles = await _selectListHelper.GetRolesSelectListAsync();

            Response<UserDTO> current = await _usersService.GetOneAsync(dto.Id);

            if (current.IsSuccess)
            {
                dto.Email = current.Result!.Email;
            }
        }
    }
}