using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Employees
{
    public class UpdateEmployeeDTO
    {
        public Guid IdEmployee { get; set; }

        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Telephone { get; set; }

        [Display(Name = "Correo electrónico")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato válido")]
        public string? Email { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public Guid IdRole { get; set; }

        public IEnumerable<SelectListItem>? Role { get; set; }
    }
}