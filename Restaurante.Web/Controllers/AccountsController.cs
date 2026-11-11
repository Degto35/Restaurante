using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.DTOs.Accounts;
using Restaurante.Web.Services;
using System.Security.Claims;

namespace Restaurante.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly Restaurante.Web.Services.AuthenticationService _authenticationService;

        public AccountsController(Restaurante.Web.Services.AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _authenticationService.AuthenticateAsync(
                model.NombreUsuario,
                model.Password);

            if (usuario == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Usuario o contrase a incorrectos.");

                return View(model);
            }

            var permisos =
                await _authenticationService.ObtenerPermisosAsync(
                    usuario.Id_Rol);

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.Id_Usuario.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    usuario.Nombre),

                new Claim(
                    ClaimTypes.Email,
                    usuario.Email),

                new Claim(
                    ClaimTypes.Role,
                    usuario.Rol?.Nombre ?? "")
            };

            foreach (var permiso in permisos)
            {
                claims.Add(new Claim("Permission", permiso));
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
