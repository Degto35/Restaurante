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
            return await _context.Roles
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetEmployeesSelectListAsync()
        {
            return await _context.Employees
                .OrderBy(e => e.Name)
                .Select(e => new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString()
                }).ToListAsync();
        }
    }
}