using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurante.Web.DTOs.Empleados;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IEmpleadosService
    {
        Task<List<EmpleadoDTO>> ObtenerTodosAsync();
        Task<EmpleadoDTO?> ObtenerPorIdAsync(Guid id);
        Task<bool> CrearAsync(CrearEmpleadoDTO dto);
        Task<bool> ActualizarAsync(ActualizarEmpleadoDTO dto);
        Task<bool> EliminarAsync(Guid id);
        Task<IEnumerable<SelectListItem>> ObtenerRolesAsync();
    }
}