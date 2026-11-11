using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.DTOs.Users;
using Restaurante.Web.Models;
using Restaurante.Web.Services;
using System.Security.Claims;

namespace Restaurante.Web.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly DataContext _context;
        private readonly PasswordService _passwordService;

        public UsuariosController(
            DataContext context,
            PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        [Authorize(Policy = "Usuarios.Ver")]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios
                .Include(x => x.Rol)
                .ToListAsync();

            return View(usuarios);
        }

        [Authorize(Policy = "Usuarios.Crear")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _context.Roles
                .Where(x => x.Activo)
                .ToListAsync();

            return View();
        }

        [Authorize(Policy = "Usuarios.Crear")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearUsuarioDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _context.Roles
                    .Where(x => x.Activo)
                    .ToListAsync();

                return View(model);
            }

            if (await _context.Cuentas.AnyAsync(
                x => x.NombreUsuario == model.NombreUsuario))
            {
                ModelState.AddModelError(
                    "NombreUsuario",
                    "El nombre de usuario ya existe.");

                ViewBag.Roles = await _context.Roles
                    .Where(x => x.Activo)
                    .ToListAsync();

                return View(model);
            }

            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Telefono = model.Telefono,
                Email = model.Email,
                Id_Rol = model.Id_Rol,
                Activo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var cuenta = new Cuenta
            {
                Id_Usuario = usuario.Id_Usuario,
                NombreUsuario = model.NombreUsuario,
                PasswordHash = _passwordService.HashPassword(
                    model.Password),
                Activa = true
            };

            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "Usuarios.Editar")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios
                .Include(x => x.Cuenta)
                .FirstOrDefaultAsync(x => x.Id_Usuario == id);

            if (usuario == null)
                return NotFound();

            ViewBag.Roles = await _context.Roles
                .Where(x => x.Activo)
                .ToListAsync();

            var model = new ActualizarUsuarioDTO
            {
                Id_Usuario = usuario.Id_Usuario,
                Nombre = usuario.Nombre,
                Telefono = usuario.Telefono,
                Email = usuario.Email,
                Id_Rol = usuario.Id_Rol,
                Activo = usuario.Activo
            };

            return View(model);
        }

        [Authorize(Policy = "Usuarios.Editar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            ActualizarUsuarioDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _context.Roles
                    .Where(x => x.Activo)
                    .ToListAsync();

                return View(model);
            }

            var usuario = await _context.Usuarios
                .Include(x => x.Cuenta)
                .FirstOrDefaultAsync(
                    x => x.Id_Usuario == model.Id_Usuario);

            if (usuario == null)
                return NotFound();

            usuario.Nombre = model.Nombre;
            usuario.Telefono = model.Telefono;
            usuario.Email = model.Email;
            usuario.Id_Rol = model.Id_Rol;
            usuario.Activo = model.Activo;

            if (!string.IsNullOrWhiteSpace(model.Password)
                && usuario.Cuenta != null)
            {
                usuario.Cuenta.PasswordHash =
                    _passwordService.HashPassword(model.Password);
            }

            if (usuario.Cuenta != null)
                usuario.Cuenta.Activa = model.Activo;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return Unauthorized();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Id_Usuario == idUsuario);

            if (usuario == null)
                return NotFound();

            var model = new PerfilDTO
            {
                Nombre = usuario.Nombre,
                Telefono = usuario.Telefono,
                Email = usuario.Email
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPerfil(
            PerfilDTO model)
        {
            if (!ModelState.IsValid)
                return View("Perfil", model);

            var idUsuario = ObtenerIdUsuario();

            if (idUsuario == null)
                return Unauthorized();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Id_Usuario == idUsuario);

            if (usuario == null)
                return NotFound();

            usuario.Nombre = model.Nombre;
            usuario.Telefono = model.Telefono;
            usuario.Email = model.Email;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Perfil));
        }

        private int? ObtenerIdUsuario()
        {
            var valor = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (int.TryParse(valor, out int id))
                return id;

            return null;
        }
    }
}
