using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Categories
{
    public class CreateCategoryDTO
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string? Description { get; set; }

    }
}
