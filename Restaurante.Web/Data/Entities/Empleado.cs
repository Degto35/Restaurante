using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.Entities
{
    public class Empleado
    {
        [Key]
        public Guid IdEmpleado { get; set; }

        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Telefono { get; set; }

        [Display(Name = "Correo electrónico")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato válido")]
        public string? Email { get; set; }

        public Guid IdRol { get; set; }

        public Rol Rol { get; set; } = null!;
    }
}