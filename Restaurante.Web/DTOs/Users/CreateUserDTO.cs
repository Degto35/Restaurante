using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Users
{
    public class CreateUserDTO
    {
        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo {0} no es un correo válido.")]
        [MaxLength(256, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string Document { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "El campo {0} no es un teléfono válido.")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid RoleId { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Confirmar contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // Only feeds the dropdown; it is not part of the entity.
        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}