using Restaurante.Web.Data.Abstractions;
using Restaurante.Web.Data.Enums;
using Restaurante.Web.Entities;            // temporal: Employees todavía vive ahí
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Web.Data.Entities
{
    public class Order : IId
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Guid DiningTableId { get; set; }
        public DiningTable DiningTable { get; set; } = null!;

        public Guid EmployeeId { get; set; }
        public Employees Employee { get; set; } = null!;   // pasará a "Employee" con F2

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public Bill? Bill { get; set; }

        [NotMapped]
        public decimal Total => OrderDetails.Sum(d => d.Subtotal);
    }
}