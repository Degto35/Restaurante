using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Categorias
{
    public class ActualizarCategoriaDTO
    {
        [Required]
        public Guid IdCategorias { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descripcion { get; set; }

        public bool Estado { get; set; }
    }
}
