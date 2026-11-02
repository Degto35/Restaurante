using System.ComponentModel.DataAnnotations;
using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Entities
{
    public class Categories
    {
        [Key]
        public Guid IdCategories { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Description { get; set; }

        public bool State { get; set; } = true;

        public ICollection<Dishes> Dishes { get; set; } = new List<Dishes>();
    }
}
