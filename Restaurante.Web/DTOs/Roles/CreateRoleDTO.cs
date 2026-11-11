using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Roles
{
    public class CreateRoleDTO
    {
        [Display(Name = "Nombre del rol")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string Name { get; set; } = string.Empty;
    }
}