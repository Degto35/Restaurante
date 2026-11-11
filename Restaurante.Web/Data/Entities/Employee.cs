using Restaurante.Web.Data.Abstractions;
using Restaurante.Web.Data.Entities;      
using System.ComponentModel.DataAnnotations;


namespace Restaurante.Web.Data.Entities
{
    public class Employee : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        public string? Telephone { get; set; }

        [Display(Name = "Correo electrónico")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato válido")]
        public string? Email { get; set; }

        public Guid RoleId { get; set; }

        public Role Role { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();  
    }
}