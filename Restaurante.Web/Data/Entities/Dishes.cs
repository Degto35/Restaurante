using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Web.Entities
{
    public class Dishes
    {
        [Key]
        public Guid IdDish { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public bool State { get; set; } = true;

        public Guid IdCategories { get; set; }

        public Categories Category { get; set; } = null!;
    }
}
