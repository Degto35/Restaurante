using Restaurante.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.Data.Entities
{
    public class DiningTable : IId
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El número de mesa es obligatorio.")]
        [Range(1, 1000, ErrorMessage = "El número debe ser mayor a 0.")]
        public int Number { get; set; }
        [Required(ErrorMessage = "La capacidad es obligatoria.")]
        [Range(1, 50, ErrorMessage = "La capacidad debe ser entre 1 y 50 personas.")]
        public int Capacity { get; set; }

        public bool IsAvailable { get; set; } = true;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
