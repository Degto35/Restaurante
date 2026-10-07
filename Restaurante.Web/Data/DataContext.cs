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
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
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


            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Rol)
                .WithMany(r => r.Empleados)
                .HasForeignKey(e => e.IdRol)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
