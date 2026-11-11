using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Web.Models
{
    public class DetallePedido
    {
        [Key]
        public int Id_DetallePedido { get; set; }

        public int Id_Pedido { get; set; }

        public int Id_Plato { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

        [ForeignKey(nameof(Id_Pedido))]
        public Pedidos? Pedidos { get; set; }

        [ForeignKey(nameof(Id_Plato))]
        public Plato? Plato { get; set; }
    }
}

