using Restaurante.Web.Data.Abstractions;
using Restaurante.Web.Entities;            // temporal: Dishes todavía vive ahí
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Web.Data.Entities
{
    public class OrderDetail : IId
    {
        [Key]
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid DishId { get; set; }
        public Dishes Dish { get; set; } = null!;           // pasará a "Dish" con F2

        [Display(Name = "Cantidad")]
        [Range(1, 100, ErrorMessage = "El campo {0} debe estar entre {1} y {2}")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal Subtotal => Quantity * UnitPrice;
    }
}