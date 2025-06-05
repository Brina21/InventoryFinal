using InventoryFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryFinal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // Definir las tablas de la base de datos //
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<MovimientoStock> MovimientoStocks { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        // Configurar las relaciones entre las tablas y otros//
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cliente 1-M Compra
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Cliente)
                .WithMany(c => c.Compras)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict); // No borrar si hay compras asociadas

            // Cliente 1-M Venta
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario 1-M Venta
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Usuario)
                .WithMany(v => v.Ventas)
                .HasForeignKey(v => v.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario 1-M Compra
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Usuario)
                .WithMany(c => c.Compras)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario 1-M MovimientoStock
            modelBuilder.Entity<MovimientoStock>()
                .HasOne(mv => mv.Usuario)
                .WithMany(mv => mv.MovimientoStocks)
                .HasForeignKey(mv => mv.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull); // Mantener el registro si se borra el usuario

            // Venta 1-M DetalleVenta
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Venta)
                .WithMany(dv => dv.DetalleVentas)
                .HasForeignKey(dv => dv.VentaId)
                .OnDelete(DeleteBehavior.Cascade); // Borrar los detalles si se borra la venta

            // Venta 1-M MovimientoStock
            modelBuilder.Entity<MovimientoStock>()
                .HasOne(ms => ms.Venta)
                .WithMany(ms => ms.MovimientoStocks)
                .HasForeignKey(ms => ms.VentaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Compra 1-M DetalleCompra
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Compra)
                .WithMany(dc => dc.DetalleCompras)
                .HasForeignKey(dc => dc.CompraId)
                .OnDelete(DeleteBehavior.Cascade);

            // Compra 1-M MovimientoStock
            modelBuilder.Entity<MovimientoStock>()
                .HasOne(ms => ms.Compra)
                .WithMany(ms => ms.MovimientoStocks)
                .HasForeignKey(ms => ms.CompraId)
                .OnDelete(DeleteBehavior.Restrict);

            // MovimientoStock M-1 Producto
            modelBuilder.Entity<MovimientoStock>()
                .HasOne(ms => ms.Producto)
                .WithMany(p => p.MovimientoStocks)
                .HasForeignKey(ms => ms.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // DetalleVenta M-1 Producto
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Producto)
                .WithMany(p => p.DetalleVentas)
                .HasForeignKey(dv => dv.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // DetalleCompra M-1 Producto
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Producto)
                .WithMany(p => p.DetalleCompras)
                .HasForeignKey(dc => dc.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Producto 1-M MovimientoStock
            modelBuilder.Entity<MovimientoStock>()
                .HasOne(ms => ms.Producto)
                .WithMany(p => p.MovimientoStocks)
                .HasForeignKey(ms => ms.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Producto M-1 Categoria
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(pr => pr.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Categoria 1-M Producto
            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Email único //
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Ajuste decimal //
            modelBuilder.Entity<Compra>()
                .Property(c => c.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetalleCompra>()
                .Property(dc => dc.PrecioUnitario)
                .HasPrecision(18, 2);
            modelBuilder.Entity<DetalleCompra>()
                .Property(dc => dc.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetalleVenta>()
                .Property(dv => dv.PrecioUnitario)
                .HasPrecision(18, 2);
            modelBuilder.Entity<DetalleVenta>()
                .Property(dv => dv.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Venta>()
                .Property(v => v.Total)
                .HasPrecision(18, 2);
            // ------------- //

            // Nombre único //
            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Nombre)
                .IsUnique();
        }
        public DbSet<InventoryFinal.DTO.DetalleCompraDTO> DetalleCompraDTO { get; set; } = default!;
    }
}