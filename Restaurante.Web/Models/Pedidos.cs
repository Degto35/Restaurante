using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.Models
{
    public class Pedidos
    {
        [Key]
        public int Id_Pedido { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public ICollection<DetallePedido> DetallesPedido { get; set; }
            = new List<DetallePedido>();
    }
}
