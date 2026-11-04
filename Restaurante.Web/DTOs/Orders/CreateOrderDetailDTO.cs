using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Orders
{
    public class CreateOrderDetailDTO
    {
        [Required]
        public Guid OrderId { get; set; }

        [Display(Name = "Plato")]
        [Required(ErrorMessage = "Debe seleccionar un plato.")]
        public Guid DishId { get; set; }

        [Display(Name = "Cantidad")]
        [Range(1, 100, ErrorMessage = "La cantidad debe estar entre {1} y {2}.")]
        public int Quantity { get; set; } = 1;
    }
}