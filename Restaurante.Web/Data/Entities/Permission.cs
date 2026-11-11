using Restaurante.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.Data.Entities
{
    public class Permission : IId
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(32)]
        public string Module { get; set; } = string.Empty;

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}