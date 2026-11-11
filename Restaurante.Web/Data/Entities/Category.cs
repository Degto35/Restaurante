using System.ComponentModel.DataAnnotations;
using Restaurante.Web.Data.Abstractions;
using Restaurante.Web.Data.Entities;
namespace Restaurante.Web.Data.Entities

{
    public class Category : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
