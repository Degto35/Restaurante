using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurante.Web.DTOs.Employees;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IEmployeesService
    {
        Task<List<EmployeesDTO>> ObtenerTodosAsync();
        Task<EmployeesDTO?> ObtenerPorIdAsync(Guid id);
        Task<bool> CrearAsync(CreateEmployeeDTO dto);
        Task<bool> ActualizarAsync(UpdateEmployeeDTO dto);
        Task<bool> EliminarAsync(Guid id);
        Task<IEnumerable<SelectListItem>> ObtenerRolesAsync();
    }
}