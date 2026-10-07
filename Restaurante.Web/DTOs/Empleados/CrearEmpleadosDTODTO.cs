using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Empleados
{
    public class CrearEmpleadoDTO
    {
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Telefono { get; set; }

        [Display(Name = "Correo electrónico")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato válido")]
        public string? Email { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public Guid IdRol { get; set; }

        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}