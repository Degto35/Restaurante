using System.ComponentModel.DataAnnotations;
using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Entities
{
    public class Categorias
    {
        [Key]
        public Guid IdCategorias { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Descripcion { get; set; }

        public bool Estado { get; set; } = true;

        public ICollection<Plato> Plato { get; set; } = new List<Plato>();
    }
}
