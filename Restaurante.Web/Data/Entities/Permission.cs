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
        public required string Code { get; set; }          

        [Required]
        [MaxLength(256)]
        public required string Description { get; set; }   

        [Required]
        [MaxLength(32)]
        public required string Module { get; set; }        

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}