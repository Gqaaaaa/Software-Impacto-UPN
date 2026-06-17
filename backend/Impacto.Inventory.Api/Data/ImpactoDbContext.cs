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
    public DbSet<Cliente> Clientes { get; set; } = null!;
    public DbSet<Proveedor> Proveedores { get; set; } = null!;
    public DbSet<Empleado> Empleados { get; set; } = null!;
    public DbSet<Rol> Roles { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<UsuarioRol> UsuarioRoles { get; set; } = null!;

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

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Dni)
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Nombres)
                .HasColumnName("nombres")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Apellidos)
                .HasColumnName("apellidos")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Dni)
                .HasColumnName("dni")
                .HasMaxLength(8)
                .IsFixedLength()
                .IsUnicode(false);

            entity.Property(e => e.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.Property(e => e.Direccion)
                .HasColumnName("direccion")
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("Proveedor");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Ruc)
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.RazonSocial)
                .HasColumnName("razonSocial")
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.Property(e => e.Ruc)
                .HasColumnName("ruc")
                .HasMaxLength(11)
                .IsFixedLength()
                .IsUnicode(false);

            entity.Property(e => e.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.Property(e => e.Direccion)
                .HasColumnName("direccion")
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.ToTable("Empleado");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Dni)
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Nombres)
                .HasColumnName("nombres")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Apellidos)
                .HasColumnName("apellidos")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Dni)
                .HasColumnName("dni")
                .HasMaxLength(8)
                .IsFixedLength()
                .IsUnicode(false);

            entity.Property(e => e.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.Property(e => e.Direccion)
                .HasColumnName("direccion")
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Nombre)
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.NombreUsuario)
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.NombreUsuario)
                .HasColumnName("usuario")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Contrasena)
                .HasColumnName("contrasena")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Estado)
                .HasColumnName("estado");

            entity.Property(e => e.IdEmpleado)
                .HasColumnName("idEmpleado")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(e => e.Empleado)
                .WithMany()
                .HasForeignKey(e => e.IdEmpleado);
        });

        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.ToTable("UsuarioRol");
            entity.HasKey(e => new { e.IdUsuario, e.IdRol });

            entity.Property(e => e.IdUsuario)
                .HasColumnName("idUsuario")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.IdRol)
                .HasColumnName("idRol")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Estado)
                .HasColumnName("estado")
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasOne(e => e.Usuario)
                .WithMany(e => e.UsuarioRoles)
                .HasForeignKey(e => e.IdUsuario);

            entity.HasOne(e => e.Rol)
                .WithMany()
                .HasForeignKey(e => e.IdRol);
        });
    }
}
