using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Orders
{
    public class CreateOrderDTO
    {
        [Required(ErrorMessage = "Debe seleccionar una mesa.")]
        public Guid IdTable { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un empleado/mesero.")]
        public Guid IdEmployee { get; set; }

        public List<CreateOrderDetailDTO> Details { get; set; } = new List<CreateOrderDetailDTO>();
    }
}