using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Data.Seeders
{
    public class DiningTablesSeeder
    {
        private readonly DataContext _context;

        public DiningTablesSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (await _context.DiningTables.AnyAsync())
            {
                return;
            }

            int[] capacities = [2, 2, 4, 4, 6, 8];

            for (int i = 0; i < capacities.Length; i++)
            {
                await _context.DiningTables.AddAsync(new DiningTable
                {
                    Id = Guid.NewGuid(),
                    Number = i + 1,
                    Capacity = capacities[i]
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}