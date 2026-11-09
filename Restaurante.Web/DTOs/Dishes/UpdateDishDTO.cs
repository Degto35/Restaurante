using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Dishes
{
    public class UpdateDishDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        [MaxLength(300, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string? Description { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 9999999.99, ErrorMessage = "El precio debe ser mayor a 0 y no superar 9.999.999.")]
        public decimal Price { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        public Guid CategoryId { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }

        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}