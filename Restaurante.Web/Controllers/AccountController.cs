using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.DTOs.Account;
using Restaurante.Web.Services.Abstractions;



namespace Restaurante.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginDTO());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO dto, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            Response response = await _accountService.LoginAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message!);
                return View(dto);
            }

           
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

       
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            Response<ProfileDTO> response = await _accountService.GetProfileAsync();

            if (!response.IsSuccess)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result!);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await FillReadOnlyFieldsAsync(dto);
                return View(dto);
            }

            Response response = await _accountService.UpdateProfileAsync(dto);

            if (!response.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, response.Message!);
                await FillReadOnlyFieldsAsync(dto);
                return View(dto);
            }

            TempData["Exito"] = response.Message;
            return RedirectToAction(nameof(Profile));
        }

        private async Task FillReadOnlyFieldsAsync(ProfileDTO dto)
        {
            Response<ProfileDTO> current = await _accountService.GetProfileAsync();

            if (current.IsSuccess)
            {
                dto.Email = current.Result!.Email;
                dto.RoleName = current.Result.RoleName;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;
            return View();
        }


    }
}