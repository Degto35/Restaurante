using Restaurante.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.Entities
{
    public class Rol
    {
        [Key]
        public Guid IdRol { get; set; }

        [Display(Name = "Nombre del rol")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NombreRol { get; set; } = string.Empty;

        public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}