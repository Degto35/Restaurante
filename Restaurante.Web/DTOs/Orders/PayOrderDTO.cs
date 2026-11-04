using Restaurante.Web.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Orders
{
    public class PayOrderDTO
    {
        [Required]
        public Guid OrderId { get; set; }

        [Display(Name = "Método de pago")]
        [Required(ErrorMessage = "Debe seleccionar un método de pago.")]
        public PaymentMethod PaymentMethod { get; set; }
    }
}