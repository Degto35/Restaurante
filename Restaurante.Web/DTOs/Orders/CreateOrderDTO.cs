using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Restaurante.Web.DTOs.Orders
{
    public class CreateOrderDTO
    {
        [Display(Name = "Mesa")]
        [Required(ErrorMessage = "Debe seleccionar una mesa.")]
        public Guid DiningTableId { get; set; }

        [Display(Name = "Mesero")]
        [Required(ErrorMessage = "Debe seleccionar un mesero.")]
        public string UserId { get; set; } = string.Empty;
        public IEnumerable<SelectListItem>? Tables { get; set; }
        public IEnumerable<SelectListItem>? Users { get; set; }
    }
}