using Microsoft.EntityFrameworkCore;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.Entities;

namespace Restaurante.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Plato> Plato { get; set; }
        // TODO: descomentar cuando se complete cada entidad
        // public DbSet<Clientes> Clientes { get; set; }
        // public DbSet<Cuentas> Cuentas { get; set; }
        // public DbSet<DetallePedido> DetallesPedido { get; set; }
        // public DbSet<Mesas> Mesas { get; set; }
        // public DbSet<Pedidos> Pedidos { get; set; }
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
