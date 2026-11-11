using Microsoft.AspNetCore.Mvc.Rendering;

namespace Restaurante.Web.Helpers.Abstractions
{
    public interface ISelectListHelper
    {
        Task<List<SelectListItem>> GetRolesSelectListAsync();
        Task<List<SelectListItem>> GetUsersSelectListAsync();
        Task<List<SelectListItem>> GetCategoriesSelectListAsync(Guid? currentCategoryId = null);
        Task<List<SelectListItem>> GetAvailableDishesSelectListAsync();
        Task<List<SelectListItem>> GetAvailableTablesSelectListAsync();
    }
}