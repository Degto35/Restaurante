using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Data.Seeders
{
    public class MenuSeeder
    {
        private readonly DataContext _context;

        public MenuSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Demo data only for an empty menu.
            if (await _context.Categories.AnyAsync())
            {
                return;
            }

            Category starters = new Category { Id = Guid.NewGuid(), Name = "Entradas", Description = "Para abrir el apetito" };
            Category mains = new Category { Id = Guid.NewGuid(), Name = "Platos fuertes", Description = "Los platos principales de la casa" };
            Category drinks = new Category { Id = Guid.NewGuid(), Name = "Bebidas", Description = "Jugos, gaseosas y más" };
            Category desserts = new Category { Id = Guid.NewGuid(), Name = "Postres", Description = "Para cerrar con algo dulce" };

            await _context.Categories.AddRangeAsync(starters, mains, drinks, desserts);

            await _context.Dishes.AddRangeAsync(
                new Dish { Id = Guid.NewGuid(), Name = "Empanadas", Description = "Empanadas de carne con ají", Price = 8000, CategoryId = starters.Id },
                new Dish { Id = Guid.NewGuid(), Name = "Patacones con hogao", Description = "Plátano verde frito con hogao", Price = 12000, CategoryId = starters.Id },
                new Dish { Id = Guid.NewGuid(), Name = "Bandeja paisa", Description = "Frijoles, arroz, carne, chicharrón, huevo y plátano", Price = 32000, CategoryId = mains.Id },
                new Dish { Id = Guid.NewGuid(), Name = "Sancocho de gallina", Description = "Sopa tradicional con gallina y verduras", Price = 28000, CategoryId = mains.Id },
                new Dish { Id = Guid.NewGuid(), Name = "Churrasco", Description = "Corte de res a la parrilla con papas", Price = 38000, CategoryId = mains.Id },
                new Dish { Id = Guid.NewGuid(), Name = "Limonada natural", Description = "Limonada recién hecha", Price = 7000, CategoryId = drinks.Id },
                new Dish { Id = Guid.NewGuid(), Name = "Jugo de mora", Description = "Jugo natural en agua o en leche", Price = 8000, CategoryId = drinks.Id },
                new Dish { Id = Guid.NewGuid(), Name = "Gaseosa", Description = "Botella personal", Price = 5000, CategoryId = drinks.Id },
                new Dish { Id = Guid.NewGuid(), Name = "Arroz con leche", Description = "Postre casero con canela", Price = 9000, CategoryId = desserts.Id },
                new Dish { Id = Guid.NewGuid(), Name = "Brownie con helado", Description = "Brownie tibio con helado de vainilla", Price = 14000, CategoryId = desserts.Id });

            await _context.SaveChangesAsync();
        }
    }
}