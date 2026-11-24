using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Core;
using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Data.Seeders
{
    public class AccountSeeder
    {
        
        private const string DEFAULT_PASSWORD = "1234";

        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public AccountSeeder(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await CheckRolesAsync();
            await CheckUsersAsync();
        }

        private async Task CheckRolesAsync()
        {
            
            await CreateRoleAsync(Env.SUPER_ADMIN_ROLE_NAME, []);

            await CreateRoleAsync(Env.WAITER_ROLE_NAME,
            [
                "orders.show", "orders.create", "orders.update",
                "dishes.show", "categories.show", "tables.show"
            ]);

            await CreateRoleAsync(Env.CASHIER_ROLE_NAME, ["orders.show", "orders.pay"]);
            await CreateRoleAsync(Env.CHEF_ROLE_NAME, ["orders.show", "orders.update"]);
        }

        private async Task CreateRoleAsync(string name, string[] permissionCodes)
        {
          
            if (await _context.RestauranteRoles.AnyAsync(r => r.Name == name))
            {
                return;
            }

            Role role = new Role { Id = Guid.NewGuid(), Name = name };
            await _context.RestauranteRoles.AddAsync(role);

            List<Permission> permissions = await _context.Permissions
                .Where(p => permissionCodes.Contains(p.Code))
                .ToListAsync();

            foreach (Permission permission in permissions)
            {
                await _context.RolePermissions.AddAsync(new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = permission.Id
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckUsersAsync()
        {
            await CreateUserAsync("admin@restaurante.com", "Administrador", "Restaurante", "1111", "3000000001", Env.SUPER_ADMIN_ROLE_NAME);
            await CreateUserAsync("laura@restaurante.com", "Laura", "Gómez", "2222", "3000000002", Env.WAITER_ROLE_NAME);
            await CreateUserAsync("andres@restaurante.com", "Andrés", "Torres", "3333", "3000000003", Env.WAITER_ROLE_NAME);
            await CreateUserAsync("pedro@restaurante.com", "Pedro", "Ramírez", "4444", "3000000004", Env.CASHIER_ROLE_NAME);
            await CreateUserAsync("maria@restaurante.com", "María", "López", "5555", "3000000005", Env.CHEF_ROLE_NAME);
        }

        private async Task CreateUserAsync(string email, string firstName, string lastName,
                                           string document, string phone, string roleName)
        {
            if (await _userManager.FindByEmailAsync(email) is not null)
            {
                return;
            }

            Role role = await _context.RestauranteRoles.FirstAsync(r => r.Name == roleName);

            User user = new User
            {
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                Document = document,
                PhoneNumber = phone,
                RoleId = role.Id,
                EmailConfirmed = true   
            };

           
            IdentityResult result = await _userManager.CreateAsync(user, DEFAULT_PASSWORD);

            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Could not seed the user {email}: {errors}");
            }
        }
    }
}