namespace Restaurante.Web.Data.Entities
{
    public class RolePermission
    {
        public required Guid RoleId { get; set; }
        public required Guid PermissionId { get; set; }
        public Role? Role { get; set; }
        public Permission? Permission { get; set; }
    }
}