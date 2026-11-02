using Restaurante.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.Entities
{
    public class Role
    {
        [Key]
        public Guid IdRole { get; set; }

        [Display(Name = "Nombre del rol")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NameRole { get; set; } = string.Empty;

        public ICollection<Employees> Employees { get; set; } = new List<Employees>();
    }
}