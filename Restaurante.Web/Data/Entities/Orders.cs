
using Restaurante.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.Data.Entities
{
    public class Orders
    {
        [Key]
        public Guid IdOrder { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "Pendiente";

        public decimal TotalAmount { get; set; }

        // Relación con Table
        public Guid IdTable { get; set; }
        public DiningTable Table { get; set; } = null!;

        // Relación con Employee (Asegúrate de que la clase se llama Employees o Employee)
        public Guid IdEmployee { get; set; }
        public Employees Employee { get; set; } = null!; // Si tu entidad de empleado se llama "Employees"

        // Relación con OrderDetail en SINGULAR
        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
}