using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Dishes
{
    public class DishDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "Precio")]
        public decimal Price { get; set; }

        [Display(Name = "Estado")]
        public bool IsActive { get; set; }

        public Guid CategoryId { get; set; }

        [Display(Name = "Categoría")]
        public string CategoryName { get; set; } = string.Empty;
    }
}