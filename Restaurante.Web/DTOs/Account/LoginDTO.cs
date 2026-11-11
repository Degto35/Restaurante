using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Account
{
    public class LoginDTO
    {
        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo {0} no es un correo válido.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}