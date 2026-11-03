using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Tables
{
    public class TableDTO
    {
        public Guid IdTable { get; set; }
        public int Number { get; set; }
        
        public int Capacity { get; set; }

        public bool IsAvailable { get; set; }
    }
}
