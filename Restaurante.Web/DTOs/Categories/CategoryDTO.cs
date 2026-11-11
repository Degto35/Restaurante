using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Categories
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "Estado")]
        public bool IsActive { get; set; }
    }
}