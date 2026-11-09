using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Employees
{
    public class CreateEmployeeDTO
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string? Telephone { get; set; }

        [Display(Name = "Correo electrónico")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato válido.")]
        public string? Email { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid RoleId { get; set; }

        // Only feeds the dropdown; it is not part of the entity.
        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}