using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Account
{
    public class ResetPasswordDTO
    {
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Display(Name = "Nueva contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Display(Name = "Confirmar nueva contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}