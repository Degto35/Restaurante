namespace Restaurante.Web.DTOs.Roles
{
    
    public class PermissionForRoleDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public bool Selected { get; set; }
    }
}