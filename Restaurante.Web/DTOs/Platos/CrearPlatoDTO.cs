using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Platos
{
    public class CrearPlatoDTO
    {
        [Required(ErrorMessage = "El nombre del plato es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 99999.99, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        public int IdCategorias { get; set; }
    }
}
