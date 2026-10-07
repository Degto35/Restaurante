using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Web.Models
{
    public class Pedidos
    {
        [Key]
        public int IdPedido { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(50)] 
        public string Estado { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18,2)")] 
        public decimal Total { get; set; }

        [Required]
        public int IdMesa { get; set; }
        [ForeignKey("IdMesa")]
        public virtual Mesa Mesa { get; set; }

        [Required]
        public int IdCliente { get; set; }
        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [Required]
        public int IdUsuario { get; set; }
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<DetallePedido> Detalles { get; set; }
    }
}