namespace Restaurante.Web.Models
{
    public class Categorias
    {
            public int IdCategorias { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string? Descripcion { get; set; }
            public bool Estado { get; set; } = true;
            public ICollection<Plato> Plato { get; set; } = new List<Plato>();   
    }
}
