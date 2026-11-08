using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Dishes
{
    public class CreateDishDTO
    {
        [Required(ErrorMessage = "El nombre del plato es obligatorio")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 9999999.99, ErrorMessage = "El precio debe ser mayor a 0 y no superar 9.999.999")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        public Guid IdCategories { get; set; }
    }
}
