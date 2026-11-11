using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.DiningTables
{
    public class DiningTableDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Número de mesa")]
        public int Number { get; set; }

        [Display(Name = "Capacidad")]
        public int Capacity { get; set; }

        [Display(Name = "Disponible")]
        public bool IsAvailable { get; set; }
    }
}