using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Account
{
    public class ProfileDTO
    {
        
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Rol")]
        public string RoleName { get; set; } = string.Empty;

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string Document { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "El campo {0} no es un teléfono válido.")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string? PhoneNumber { get; set; }
    }
}