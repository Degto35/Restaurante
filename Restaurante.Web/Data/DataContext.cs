using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Models;

namespace Restaurante.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Plato> Plato { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Plato>()
                .HasOne(p => p.Categorias)          // Un Plato tiene una Categoria
                .WithMany(c => c.Plato)           // Una Categoria tiene muchos Platos
                .HasForeignKey(p => p.IdCategorias) // La FK es IdCategoria en Plato
                .OnDelete(DeleteBehavior.Restrict); // Evita borrar la categoría si tiene platos asociados
        }
    }
}
