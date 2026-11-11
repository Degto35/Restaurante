using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.DTOs.Roles;
using Restaurante.Web.Models;

namespace Restaurante.Web.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly DataContext _context;

        public RolesController(DataContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Roles.Ver")]
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles
                .Include(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .ToListAsync();

            return View(roles);
        }

        [Authorize(Policy = "Roles.Crear")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Permisos =
                await _context.Permissions.ToListAsync();

            return View();
        }

        [Authorize(Policy = "Roles.Crear")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CrearRolDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Permisos =
                    await _context.Permissions.ToListAsync();

                return View(model);
            }

            if (await _context.Roles.AnyAsync(
                x => x.Nombre == model.Nombre))
            {
                ModelState.AddModelError(
                    "Nombre",
                    "El rol ya existe.");

                ViewBag.Permisos =
                    await _context.Permissions.ToListAsync();

                return View(model);
            }

            var rol = new Rol
            {
                Nombre = model.Nombre,
                Activo = true
            };

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            foreach (var idPermiso in model.PermisosSeleccionados)
            {
                _context.RolePermissions.Add(
                    new RolePermission
                    {
                        Id_Rol = rol.Id_Rol,
                        Id_Permiso = idPermiso
                    });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "Roles.Editar")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var rol = await _context.Roles
                .Include(x => x.RolePermissions)
                .FirstOrDefaultAsync(x => x.Id_Rol == id);

            if (rol == null)
                return NotFound();

            ViewBag.Permisos =
                await _context.Permissions.ToListAsync();

            var model = new ActualizarRolDTO
            {
                Id_Rol = rol.Id_Rol,
                Nombre = rol.Nombre,
                Activo = rol.Activo,
                PermisosSeleccionados =
                    rol.RolePermissions
                       .Select(x => x.Id_Permiso)
                       .ToList()
            };

            return View(model);
        }

        [Authorize(Policy = "Roles.Editar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            ActualizarRolDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Permisos =
                    await _context.Permissions.ToListAsync();

                return View(model);
            }

            var rol = await _context.Roles
                .Include(x => x.RolePermissions)
                .FirstOrDefaultAsync(
                    x => x.Id_Rol == model.Id_Rol);

            if (rol == null)
                return NotFound();

            rol.Nombre = model.Nombre;
            rol.Activo = model.Activo;

            _context.RolePermissions.RemoveRange(
                rol.RolePermissions);

            foreach (var idPermiso in model.PermisosSeleccionados)
            {
                _context.RolePermissions.Add(
                    new RolePermission
                    {
                        Id_Rol = rol.Id_Rol,
                        Id_Permiso = idPermiso
                    });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
