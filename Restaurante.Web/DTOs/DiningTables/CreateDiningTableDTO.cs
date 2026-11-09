using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.DiningTables
{
    public class CreateDiningTableDTO
    {
        [Display(Name = "Número de mesa")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, 1000, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
        public int Number { get; set; }

        [Display(Name = "Capacidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, 50, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
        public int Capacity { get; set; }
    }
}