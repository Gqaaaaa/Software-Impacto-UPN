using Impacto.Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Impacto.Inventory.Api.Data;

public class ImpactoDbContext : DbContext
{
    public ImpactoDbContext(DbContextOptions<ImpactoDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; } = null!;
    public DbSet<Producto> Productos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categoria");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Descripcion)
                .HasColumnName("descripcion")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Descripcion)
                .HasColumnName("descripcion")
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.Property(e => e.PrecioCompra)
                .HasColumnName("precio_compra")
                .HasColumnType("decimal(10,2)");

            entity.Property(e => e.PrecioVenta)
                .HasColumnName("precio_venta")
                .HasColumnType("decimal(10,2)");

            entity.Property(e => e.Stock)
                .HasColumnName("stock");

            entity.Property(e => e.IdCategoria)
                .HasColumnName("idCategoria")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(e => e.Categoria)
                .WithMany()
                .HasForeignKey(e => e.IdCategoria);
        });
    }
}
