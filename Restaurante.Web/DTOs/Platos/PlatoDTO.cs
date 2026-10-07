namespace Restaurante.Web.DTOs.Platos
{
    public class PlatoDTO
    {
        public Guid IdPlato { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; }
        public Guid IdCategorias { get; set; }
        public string NombreCategorias { get; set; } = string.Empty;
    }
}
