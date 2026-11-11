namespace Restaurante.Web.Models
{
    public class Permission
    {
        public int Id_Permiso { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Codigo { get; set; } = string.Empty;

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
