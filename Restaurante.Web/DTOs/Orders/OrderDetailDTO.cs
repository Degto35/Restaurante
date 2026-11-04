namespace Restaurante.Web.DTOs.Orders
{
    public class OrderDetailDTO
    {
        public Guid Id { get; set; }
        public Guid DishId { get; set; }
        public string DishName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal Subtotal => Quantity * UnitPrice;
    }
}
