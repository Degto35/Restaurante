using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Orders
{
    public class CreateOrderDetailDTO
    {
        [Required(ErrorMessage = "Debe seleccionar un plato.")]
        public Guid IdDish { get; set; }

        [Range(1, 100, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Quantity { get; set; } = 1;
    }
}