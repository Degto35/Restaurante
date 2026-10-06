namespace Restaurante.Web.Models
{
    public class Plato
    {
        public int IdPlato { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; } = true;
        public int IdCategorias { get; set; }
        public Categorias Categorias { get; set; } = null!;
    }
}
