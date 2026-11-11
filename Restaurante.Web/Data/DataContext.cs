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

        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Plato> Platos { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasKey(x => x.Id_Usuario);

            modelBuilder.Entity<Rol>()
                .HasKey(x => x.Id_Rol);

            modelBuilder.Entity<Cuenta>()
                .HasKey(x => x.Id_Cuenta);

            modelBuilder.Entity<Permission>()
                .HasKey(x => x.Id_Permiso);

            modelBuilder.Entity<RolePermission>()
                .HasKey(x => new { x.Id_Rol, x.Id_Permiso });

            modelBuilder.Entity<Usuario>()
                .HasOne(x => x.Rol)
                .WithMany(x => x.Usuarios)
                .HasForeignKey(x => x.Id_Rol)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cuenta>()
                .HasOne(x => x.Usuario)
                .WithOne(x => x.Cuenta)
                .HasForeignKey<Cuenta>(x => x.Id_Usuario)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(x => x.Rol)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.Id_Rol);

            modelBuilder.Entity<RolePermission>()
                .HasOne(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.Id_Permiso);

            modelBuilder.Entity<Cuenta>()
                .HasIndex(x => x.NombreUsuario)
                .IsUnique();

            modelBuilder.Entity<Permission>()
                .HasIndex(x => x.Codigo)
                .IsUnique();

            // Categorias - Plato
            modelBuilder.Entity<Plato>()
                .HasOne(p => p.Categorias)
                .WithMany(c => c.Platos)
                .HasForeignKey(p => p.Id_Categoria)
                .OnDelete(DeleteBehavior.Restrict);

            // Pedidos - DetallePedido
            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Pedidos)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(d => d.Id_Pedido)
                .OnDelete(DeleteBehavior.Cascade);

            // Plato - DetallePedido
            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Plato)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(d => d.Id_Plato)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

