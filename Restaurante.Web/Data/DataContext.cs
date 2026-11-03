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

        public DbSet<Categories> Categories{ get; set; }
        public DbSet<Dishes> Dishes { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Employees> Employees { get; set; }
        // public DbSet<Clientes> Clientes { get; set; }
        // public DbSet<Accounts> Accounts { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
         public DbSet<DiningTable> Tables { get; set; }
        public DbSet<Orders> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Dishes>()
                .HasOne(d => d.Category)          // Un Plato tiene una Categoria
                .WithMany(c => c.Dishes)           // Una Categoria tiene muchos Platos
                .HasForeignKey(d => d.IdCategories) // La FK es IdCategoria en Plato
                .OnDelete(DeleteBehavior.Restrict); // Evita borrar la categoría si tiene platos asociados


            modelBuilder.Entity<Employees>()
                .HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.IdRole)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
