using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Users
{
    public class UpdateUserDTO
    {
        [Required]
        public string Id { get; set; } = string.Empty;

       
        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }

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

       
        [Display(Name = "Nueva contraseña")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Display(Name = "Confirmar nueva contraseña")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmNewPassword { get; set; }

        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}