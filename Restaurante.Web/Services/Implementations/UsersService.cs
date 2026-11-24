using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.DTOs.Users;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class UsersService : CustomQueryableOperationsService, IUsersService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAccountService _accountService;

        public UsersService(DataContext context, IMapper mapper, UserManager<User> userManager,
                            IAccountService accountService)
            : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _accountService = accountService;
        }

        public async Task<Response<PaginationResponse<UserDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<User> query = _context.Users
                .Include(u => u.Role)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                string filter = request.Filter.ToLower();

                query = query.Where(u => u.FirstName.ToLower().Contains(filter)
                                      || u.LastName.ToLower().Contains(filter)
                                      || u.Document.ToLower().Contains(filter)
                                      || (u.Email != null && u.Email.ToLower().Contains(filter))
                                      || u.Role!.Name.ToLower().Contains(filter));
            }

            query = query.OrderBy(u => u.FirstName).ThenBy(u => u.LastName);

            return await GetPagedListAsync<UserDTO, User>(request, query);
        }

  
        public async Task<Response<UserDTO>> GetOneAsync(string id)
        {
            try
            {
                User? user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user is null)
                {
                    return Response<UserDTO>.Failure("No existe el usuario solicitado.");
                }

                return Response<UserDTO>.Success(_mapper.Map<UserDTO>(user));
            }
            catch (Exception ex)
            {
                return Response<UserDTO>.Failure(ex);
            }
        }

        public async Task<Response> CreateAsync(CreateUserDTO dto)
        {
            try
            {
                Role? role = await _context.RestauranteRoles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);

                if (role is null)
                {
                    return Response.Failure("El rol seleccionado no existe.");
                }

                if (role.Name == Env.SUPER_ADMIN_ROLE_NAME && !await _accountService.AccountUserIsSuperAdminAsync())
                {
                    return Response.Failure("Solo un administrador puede asignar el rol de administrador.");
                }

                if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                {
                    return Response.Failure("Ya existe un usuario con ese correo.");
                }

                if (await _context.Users.AnyAsync(u => u.Document == dto.Document))
                {
                    return Response.Failure("Ya existe un usuario con ese documento.");
                }

                User user = new User
                {
                    Email = dto.Email,
                    UserName = dto.Email,   
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Document = dto.Document,
                    PhoneNumber = dto.PhoneNumber,
                    RoleId = dto.RoleId,
                    EmailConfirmed = true  
                };

              
                IdentityResult result = await _userManager.CreateAsync(user, dto.Password);

                if (!result.Succeeded)
                {
                    List<string> errors = result.Errors.Select(e => e.Description).ToList();
                    return Response.Failure("No se pudo crear el usuario.", errors);
                }

                return Response.Success("Usuario creado correctamente.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }

        public async Task<Response> UpdateAsync(UpdateUserDTO dto)
        {
            try
            {
                User? user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == dto.Id);

                if (user is null)
                {
                    return Response.Failure("No existe el usuario solicitado.");
                }

                Role? newRole = await _context.RestauranteRoles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);

                if (newRole is null)
                {
                    return Response.Failure("El rol seleccionado no existe.");
                }

                // Rule 1: only the super admin touches admin accounts or hands out the admin role.
                bool targetIsAdmin = user.Role!.Name == Env.SUPER_ADMIN_ROLE_NAME;
                bool newRoleIsAdmin = newRole.Name == Env.SUPER_ADMIN_ROLE_NAME;

                if ((targetIsAdmin || newRoleIsAdmin) && !await _accountService.AccountUserIsSuperAdminAsync())
                {
                    return Response.Failure("Solo un administrador puede modificar usuarios con rol de administrador o asignar ese rol.");
                }

                // Rule 2: nobody changes their own role.
                if (user.Id == _accountService.GetCurrentUserId() && user.RoleId != dto.RoleId)
                {
                    return Response.Failure("No puedes cambiar tu propio rol.");
                }

                if (await _context.Users.AnyAsync(u => u.Document == dto.Document && u.Id != user.Id))
                {
                    return Response.Failure("Ya existe otro usuario con ese documento.");
                }

                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Document = dto.Document;
                user.PhoneNumber = dto.PhoneNumber;
                user.RoleId = dto.RoleId;

                if (string.IsNullOrWhiteSpace(dto.NewPassword))
                {
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // All or nothing: if the new password is rejected, no other change is saved either.
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    IdentityResult result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

                    if (!result.Succeeded)
                    {
                        List<string> errors = result.Errors.Select(e => e.Description).ToList();
                        return Response.Failure("No se pudo cambiar la contraseña.", errors);
                    }
                }

                return Response.Success("Usuario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }

        public async Task<Response> DeleteAsync(string id)
        {
            try
            {
                User? user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user is null)
                {
                    return Response.Failure("No existe el usuario solicitado.");
                }

                if (user.Id == _accountService.GetCurrentUserId())
                {
                    return Response.Failure("No puedes eliminar tu propio usuario.");
                }

                if (user.Role!.Name == Env.SUPER_ADMIN_ROLE_NAME && !await _accountService.AccountUserIsSuperAdminAsync())
                {
                    return Response.Failure("Solo un administrador puede eliminar a otro administrador.");
                }

                if (await _context.Orders.AnyAsync(o => o.UserId == id))
                {
                    return Response.Failure("No se puede eliminar el usuario porque tiene pedidos asociados.");
                }

                IdentityResult result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    List<string> errors = result.Errors.Select(e => e.Description).ToList();
                    return Response.Failure("No se pudo eliminar el usuario.", errors);
                }

                return Response.Success("Usuario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }
    }
}