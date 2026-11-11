using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Web.Models
{
    public class DetallePedido
    {
        [Key]
        public int IdDetalle { get; set; }

        [Required]
        public int IdPedido { get; set; }
        [ForeignKey("IdPedido")]
        public virtual Pedidos Pedido { get; set; }

        [Required]
        public int IdPlato { get; set; }
        [ForeignKey("IdPlato")]
        public virtual Plato Plato { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
    }
}