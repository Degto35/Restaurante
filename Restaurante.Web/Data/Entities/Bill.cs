using Restaurante.Web.Data.Abstractions;
using Restaurante.Web.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Web.Data.Entities
{
    public class Bill : IId
    {
        [Key]
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public DateTime PaidAt { get; set; } = DateTime.Now;

        public PaymentMethod PaymentMethod { get; set; }

        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }
    }
}