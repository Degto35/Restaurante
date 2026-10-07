using Restaurante.Web.Data.Entities;

namespace Restaurante.Web.Entities
{
    public class DetallePedido
    {
        public Guid IdDetalle { get; set; }
        public Guid IdPedido { get; set; }
        public Guid IdPlato { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }

       public Plato Plato { get; set; } = null!;
            
    }
}
