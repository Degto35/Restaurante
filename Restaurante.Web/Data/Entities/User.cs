using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(32)]
        public required string Document { get; set; }

        [Required]
        [MaxLength(64)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(64)]
        public required string LastName { get; set; }

        public required Guid RoleId { get; set; }

        public Role? Role { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
