using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.Data.Entities
{
    public class user
    {
        [Key]
        public Guid IdUser { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? phone { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Email { get; set; } = string.Empty;

    }
}
