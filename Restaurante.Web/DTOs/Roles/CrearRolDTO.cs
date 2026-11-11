namespace Restaurante.Web.DTOs.Roles
{
    public class CrearRolDTO
    {
        public string Nombre { get; set; } = string.Empty;

        public List<int> PermisosSeleccionados { get; set; } = new();
    }
}
