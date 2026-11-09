using Restaurante.Web.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Restaurante.Web.DTOs.Orders
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public DateTime? PaidAt { get; set; }

        public Guid DiningTableId { get; set; }
        public int TableNumber { get; set; }

        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;

        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();

        public decimal Total => OrderDetails.Sum(d => d.Subtotal);

        public bool IsClosed => Status == OrderStatus.Paid || Status == OrderStatus.Cancelled;
        public IEnumerable<SelectListItem>? Dishes { get; set; }
    }
}