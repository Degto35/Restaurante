namespace Restaurante.Web.DTOs.Roles
{
    public class ActualizarRolDTO
    {
        public int Id_Rol { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; }

        public List<int> PermisosSeleccionados { get; set; } = new();
    }
}
