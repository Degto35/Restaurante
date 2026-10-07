using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Roles
{
    public class ActualizarRolDTO
    {
        public Guid IdRol { get; set; }

        [Display(Name = "Nombre del rol")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NombreRol { get; set; } = string.Empty;
    }
}
