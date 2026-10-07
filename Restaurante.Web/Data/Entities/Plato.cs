using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Web.Entities
{
    public class Plato
    {
        [Key]
        public Guid IdPlato { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(300, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public bool Estado { get; set; } = true;

        public Guid IdCategorias { get; set; }

        public Categorias Categorias { get; set; } = null!;
    }
}
