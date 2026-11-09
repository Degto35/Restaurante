using Microsoft.AspNetCore.Mvc.Rendering;

namespace Restaurante.Web.Helpers.Abstractions
{
    public interface ISelectListHelper
    {
        Task<List<SelectListItem>> GetRolesSelectListAsync();
        Task<List<SelectListItem>> GetEmployeesSelectListAsync();
    }
}