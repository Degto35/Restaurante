namespace Restaurante.Web.Models
{
    public class Rol
    {
        public int Id_Rol { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
