using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.Models
{
    public class Categorias
    {
        [Key]
        public int Id_Categoria { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Plato> Platos { get; set; }
            = new List<Plato>();
    }
}
