using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Role
{
    public class CreateRoleDTO
    {
        [Display(Name = "Nombre del rol")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NameRole { get; set; } = string.Empty;
    }
}