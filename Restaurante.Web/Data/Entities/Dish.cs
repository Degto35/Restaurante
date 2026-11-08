using Restaurante.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Web.Data.Entities
{
    public class Dish : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;
    }
}
