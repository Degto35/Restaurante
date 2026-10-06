namespace Restaurante.Web.DTOs.Categorias
{
    public class CategoriaDTO
    {
        public int IdCategorias { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
