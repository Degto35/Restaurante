using Restaurante.Web.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Orders
{
    public class UpdateOrderDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Display(Name = "Estado")]
        public OrderStatus Status { get; set; }
    }
}