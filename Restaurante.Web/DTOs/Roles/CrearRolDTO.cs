using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Roles
{
    public class CrearRolDTO
    {
        [Display(Name = "Nombre del rol")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NombreRol { get; set; } = string.Empty;
    }
}