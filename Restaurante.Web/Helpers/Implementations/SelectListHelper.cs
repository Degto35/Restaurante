using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data;
using Restaurante.Web.Helpers.Abstractions;

namespace Restaurante.Web.Helpers.Implementations
{
    public class SelectListHelper : ISelectListHelper
    {
        private readonly DataContext _context;

        public SelectListHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<List<SelectListItem>> GetRolesSelectListAsync()
        {
            return await _context.RestauranteRoles
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetUsersSelectListAsync()
        {
            return await _context.Users
                .OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
                .Select(u => new SelectListItem
                {
                    
                    Text = u.FirstName + " " + u.LastName,
                    Value = u.Id
                }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetCategoriesSelectListAsync(Guid? currentCategoryId = null)
        {
            return await _context.Categories
                .Where(c => c.IsActive || c.Id == currentCategoryId)
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Text = c.IsActive ? c.Name : c.Name + " (inactiva)",
                    Value = c.Id.ToString()
                }).ToListAsync();
        }

        
        public async Task<List<SelectListItem>> GetAvailableDishesSelectListAsync()
        {
            var dishes = await _context.Dishes
                .Where(d => d.IsActive && d.Category.IsActive)
                .OrderBy(d => d.Name)
                .Select(d => new { d.Id, d.Name, d.Price })
                .ToListAsync();

            
            return dishes.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.Name} - {d.Price:C}"
            }).ToList();
        }

        public async Task<List<SelectListItem>> GetAvailableTablesSelectListAsync()
        {
            return await _context.DiningTables
                .Where(t => t.IsAvailable)
                .OrderBy(t => t.Number)
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = "Mesa " + t.Number + " (" + t.Capacity + " personas)"
                }).ToListAsync();
        }
    }
}