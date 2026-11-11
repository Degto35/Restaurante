namespace Restaurante.Web.Models
{
    public class RolePermission
    {
        public int Id_Rol { get; set; }

        public int Id_Permiso { get; set; }

        public Rol? Rol { get; set; }

        public Permission? Permission { get; set; }
    }
}
