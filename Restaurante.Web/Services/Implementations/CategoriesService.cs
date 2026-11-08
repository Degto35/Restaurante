using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.DTOs.Categories;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class CategoriesService : ICategoriesService
    {
        private readonly DataContext _context;

        public CategoriesService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDTO>> ObtenerTodasAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryDTO
                {
                    IdCategories = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    State= c.IsActive
                }).ToListAsync();
        }

        public async Task<CategoryDTO?> ObtenerPorIdAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;

            return new CategoryDTO
            {
                IdCategories = category.Id,
                Name = category.Name,
                Description = category.Description,
                State= category.IsActive
            };
        }

        public async Task<bool> CrearAsync(CreateCategoryDTO dto)
        {

            var categories = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true
            };

            _context.Categories.Add(categories);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarAsync(UpdateCategoryDTO dto)
        {
            var category = await _context.Categories.FindAsync(dto.IdCategories);
            if (category == null) return false;

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.IsActive = dto.State;

            _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;
            var hasDishes = await _context.Dishes.AnyAsync(p => p.CategoryId == id);
            if (hasDishes) return false;

            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}