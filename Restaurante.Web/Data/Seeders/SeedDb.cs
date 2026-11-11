using Microsoft.AspNetCore.Identity;
using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public SeedDb(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
           
            await new PermissionsSeeder(_context).SeedAsync();
            await new AccountSeeder(_context, _userManager).SeedAsync();
            await new MenuSeeder(_context).SeedAsync();
            await new DiningTablesSeeder(_context).SeedAsync();
        }
    }
}