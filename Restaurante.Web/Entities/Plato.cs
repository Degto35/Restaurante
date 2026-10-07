using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.Models
{
    public class Plato
    {
        [Key]
        public int IdPlato { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; } = true;
        public int IdCategorias { get; set; }
        public Categorias Categorias { get; set; } = null!;
    }
}
