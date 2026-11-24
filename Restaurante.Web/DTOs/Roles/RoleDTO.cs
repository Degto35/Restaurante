using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Roles
{
    public class RoleDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombre del rol")]
        public string Name { get; set; } = string.Empty;

      
        public List<PermissionForRoleDTO> Permissions { get; set; } = [];
    }
}