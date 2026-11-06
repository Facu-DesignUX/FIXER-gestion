using FIXER.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIXER.Infrastructure.Persistence;

public class FixerDbContext : DbContext
{
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Merma> Mermas => Set<Merma>();

    public FixerDbContext(DbContextOptions<FixerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración Tabla CATEGORIA
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("CATEGORIA");
            entity.HasKey(e => e.IdCategoria);
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.NombreCategoria).HasColumnName("nombre_categoria").HasMaxLength(50).IsRequired();
            entity.HasIndex(e => e.NombreCategoria).IsUnique();
            entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(150);
            entity.Property(e => e.EstadoActivo).HasColumnName("estado_activo").HasDefaultValue(true);
        });

        // Configuración Tabla PRODUCTO
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("PRODUCTO");
            entity.HasKey(e => e.IdProducto);
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            entity.Property(e => e.PrecioVenta).HasColumnName("precio_venta").HasPrecision(10, 2).IsRequired();
            entity.Property(e => e.Costo).HasColumnName("costo").HasPrecision(10, 2).IsRequired();
            entity.Property(e => e.Stock).HasColumnName("stock").HasPrecision(10, 3).IsRequired();
            entity.Property(e => e.StockMinimo).HasColumnName("stock_minimo").HasPrecision(10, 3).IsRequired();
            entity.Property(e => e.Proveedor).HasColumnName("proveedor").HasMaxLength(100).IsRequired();
            entity.Property(e => e.UbicacionFisica).HasColumnName("ubicacion_fisica").HasMaxLength(50).IsRequired();
            entity.Property(e => e.UnidadMedida).HasColumnName("unidad_medida").HasMaxLength(20).IsRequired();
            entity.Property(e => e.EstadoActivo).HasColumnName("estado_activo").HasDefaultValue(true);
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria").IsRequired();

            entity.HasOne(e => e.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(e => e.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración Tabla MERMA
        modelBuilder.Entity<Merma>(entity =>
        {
            entity.ToTable("MERMA");
            entity.HasKey(e => e.IdMerma);
            entity.Property(e => e.IdMerma).HasColumnName("id_merma");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto").IsRequired();
            entity.Property(e => e.Cantidad).HasColumnName("cantidad").HasPrecision(10, 3).IsRequired();
            entity.Property(e => e.Justificacion).HasColumnName("justificacion").HasMaxLength(255).IsRequired();
            entity.Property(e => e.FechaHora).HasColumnName("fecha_hora").IsRequired();

            entity.HasOne(e => e.Producto)
                .WithMany()
                .HasForeignKey(e => e.IdProducto)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

