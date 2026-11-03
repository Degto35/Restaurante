using Restaurante.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.Data.Entities
{
    public class OrderDetails
    {
        [Key]
        public Guid IdOrderDetail { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal => Quantity * UnitPrice;

        // Relación con Order
        public Guid IdOrder { get; set; }
        public Orders Order { get; set; } = null!;

        // Relación con Dish (Plato)
        public Guid IdDish { get; set; }
        public Dishes Dish { get; set; } = null!;
    }
}