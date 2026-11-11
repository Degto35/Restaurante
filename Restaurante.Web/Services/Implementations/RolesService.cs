using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.Data;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.DTOs.Roles;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class RolesService : CustomQueryableOperationsService, IRolesService
    {
        private readonly DataContext _context;

        public RolesService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }

        public async Task<Response<PaginationResponse<RoleDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<Role> query = _context.RestauranteRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                string filter = request.Filter.ToLower();
                query = query.Where(r => r.Name.ToLower().Contains(filter));
            }

            query = query.OrderBy(r => r.Name);

            return await GetPagedListAsync<RoleDTO, Role>(request, query);
        }

        public async Task<Response<RoleDTO>> GetOneAsync(Guid id)
        {
            Response<RoleDTO> response = await GetOneAsync<RoleDTO, Role>(id);

            if (!response.IsSuccess)
            {
                return response;
            }

            RoleDTO dto = response.Result!;

            List<Guid> selectedIds = await _context.RolePermissions
                .Where(rp => rp.RoleId == id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            dto.Permissions = await BuildPermissionListAsync(selectedIds);

            return Response<RoleDTO>.Success(dto);
        }

        public async Task<Response<CreateRoleDTO>> CreateAsync(CreateRoleDTO dto)
        {
            try
            {
                if (await _context.RestauranteRoles.AnyAsync(r => r.Name == dto.Name))
                {
                    return Response<CreateRoleDTO>.Failure("Ya existe un rol con ese nombre.");
                }

                
                List<Guid> permissionIds = dto.PermissionIds.Distinct().ToList();

                int found = await _context.Permissions.CountAsync(p => permissionIds.Contains(p.Id));

                if (found != permissionIds.Count)
                {
                    return Response<CreateRoleDTO>.Failure("Alguno de los permisos seleccionados no existe.");
                }

                Role role = new Role { Id = Guid.NewGuid(), Name = dto.Name };
                await _context.RestauranteRoles.AddAsync(role);

                foreach (Guid permissionId in permissionIds)
                {
                    await _context.RolePermissions.AddAsync(new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permissionId
                    });
                }

                
                await _context.SaveChangesAsync();

                return Response<CreateRoleDTO>.Success(dto, "Rol creado con éxito");
            }
            catch (Exception ex)
            {
                return Response<CreateRoleDTO>.Failure(ex);
            }
        }

        public async Task<Response<UpdateRoleDTO>> UpdateAsync(UpdateRoleDTO dto)
        {
            return await UpdateAsync<UpdateRoleDTO, Role>(dto, dto.Id);
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            bool hasUsers = await _context.Users.AnyAsync(u => u.RoleId == id);

            if (hasUsers)
            {
                return Response.Failure("No se puede eliminar el rol porque tiene empleados asociados.");
            }

            return await DeleteAsync<Role>(id);
        }
        public async Task<Response<List<PermissionForRoleDTO>>> GetPermissionsAsync(IEnumerable<Guid>? selectedIds = null)
        {
            try
            {
                List<PermissionForRoleDTO> permissions =
                    await BuildPermissionListAsync(selectedIds ?? Enumerable.Empty<Guid>());

                return Response<List<PermissionForRoleDTO>>.Success(permissions);
            }
            catch (Exception ex)
            {
                return Response<List<PermissionForRoleDTO>>.Failure(ex);
            }
        }

        
        private async Task<List<PermissionForRoleDTO>> BuildPermissionListAsync(IEnumerable<Guid> selectedIds)
        {
            HashSet<Guid> selected = selectedIds.ToHashSet();

            List<Permission> permissions = await _context.Permissions
                .OrderBy(p => p.Module)
                .ThenBy(p => p.Code)
                .ToListAsync();

            return permissions.Select(p => new PermissionForRoleDTO
            {
                Id = p.Id,
                Code = p.Code,
                Description = p.Description,
                Module = p.Module,
                Selected = selected.Contains(p.Id)
            }).ToList();
        }
    }
}