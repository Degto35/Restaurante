namespace Restaurante.Web.DTOs.Orders
{
    public class OrderDetailDTO
    {
        public Guid IdOrderDetail { get; set; }
        public Guid IdDish { get; set; }
        public string DishName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
