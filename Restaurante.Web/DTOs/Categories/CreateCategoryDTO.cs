using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Categories
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres")]
        public string? Description { get; set; }

        // El estado por defecto será true en el Service, no es necesario pedirlo en el formulario de creación
    }
}
