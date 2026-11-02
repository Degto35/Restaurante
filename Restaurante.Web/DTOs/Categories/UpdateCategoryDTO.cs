using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Categories
{
    public class UpdateCategoryDTO
    {
        [Required]
        public Guid IdCategories { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        public bool State { get; set; }
    }
}
