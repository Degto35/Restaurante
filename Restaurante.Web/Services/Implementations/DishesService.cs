using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.DTOs.Dishes;
using Restaurante.Web.Entities;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class DishesService : IDishesService
    {
        private readonly DataContext _context;

        public DishesService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<DishDTO>> ObtenerTodosAsync()
        {
            return await _context.Dishes
                .Include(d => d.Category) // ¡IMPORTANTE! Trae los datos de la categoría
                .Select(d => new DishDTO
                {
                    IdDish = d.IdDish,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    State = d.State,
                    IdCategories = d.IdCategories,
                    CategoryName = d.Category.Name // Se llena gracias al Include
                }).ToListAsync();
        }

        public async Task<DishDTO?> ObtenerPorIdAsync(Guid id)
        {
            var dishes = await _context.Dishes
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.IdDish == id);

            if (dishes == null) return null;

            return new DishDTO
            {
                IdDish = dishes.IdDish,
                Name = dishes.Name,
                Description = dishes.Description,
                Price = dishes.Price,
                State = dishes.State,
                IdCategories = dishes.IdCategories,
                CategoryName = dishes.Category.Name
            };
        }

        public async Task<bool> CrearAsync(CreateDishDTO dto)
        {
            var dishes = new Dishes
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IdCategories = dto.IdCategories,
                State = true
            };

            _context.Dishes.Add(dishes);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(UpdateDishDTO dto)
        {
            var dishes = await _context.Dishes.FindAsync(dto.IdDish);
            if (dishes == null) return false;

            dishes.Name = dto.Name;
            dishes.Description = dto.Description;
            dishes.Price = dto.Price;
            dishes.IdCategories = dto.IdCategories;
            dishes.State = dto.State;

            _context.Dishes.Update(dishes);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(Guid id)
        {
            var dishes = await _context.Dishes.FindAsync(id);
            if (dishes == null) return false;

            //var estaEnPedido = await _context.DetallesPedido.AnyAsync(d => d.IdDish == id);
            //if (estaEnPedido) return false;

            _context.Dishes.Remove(dishes);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}