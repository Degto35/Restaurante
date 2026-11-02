using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Entities
{
    public class Orderdetails
    {
        public Guid IdDetail { get; set; }
        public Guid IdOrder { get; set; }
        public Guid IdDish { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }

       public Dishes Dish { get; set; } = null!;
            
    }
}
