namespace Restaurante.Web.DTOs.Orders
{
    public class OrderDTO
    {
        public Guid IdOrder { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        public Guid IdTable { get; set; }
        public int TableNumber { get; set; }

        public Guid IdEmployee { get; set; }
        public string EmployeeName { get; set; } = string.Empty;

        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }
}