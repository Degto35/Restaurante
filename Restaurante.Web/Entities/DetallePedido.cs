using Restaurante.Web.Entities;

namespace Restaurante.Web.Models
{
    namespace Restaurante.Web.Models
    {
        public class DetallePedido
        {
            public int IdDetalle { get; set; }
            public int IdPedido { get; set; }
            public int IdPlato { get; set; }
            public int Cantidad { get; set; }
            public decimal Subtotal { get; set; }

            public Plato Plato { get; set; } = null!;
            // Pedido Pedido se agrega cuando exista el modelo Pedido
        }
    }
}
