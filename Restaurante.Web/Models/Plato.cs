using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Web.Models
{
    public class Plato
    {
        [Key]
        public int Id_Plato { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public bool Disponible { get; set; } = true;

        public int Id_Categoria { get; set; }

        [ForeignKey(nameof(Id_Categoria))]
        public Categorias? Categorias { get; set; }

        public ICollection<DetallePedido> DetallesPedido { get; set; }
            = new List<DetallePedido>();
    }
}

