
using Microsoft.EntityFrameworkCore;
using TallerMecanicoAPI.Models;

namespace TallerMecanicoAPI.Data;

public partial class TallerMecanicoContext : DbContext
{
    public TallerMecanicoContext(DbContextOptions<TallerMecanicoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<OrdenesServicio> OrdenesServicios { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Clientes");

            entity.HasKey(e => e.IdCliente);

            entity.HasIndex(e => e.Correo).IsUnique();

            entity.Property(e => e.Apellido).HasMaxLength(60);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(60);
            entity.Property(e => e.Telefono).HasMaxLength(15);
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.ToTable("Vehiculos");

            entity.HasKey(e => e.IdVehiculo);

            entity.HasIndex(e => e.Placa).IsUnique();

            entity.Property(e => e.Marca).HasMaxLength(50);
            entity.Property(e => e.Modelo).HasMaxLength(50);
            entity.Property(e => e.Placa).HasMaxLength(10);

            entity.HasOne(d => d.IdClienteNavigation)
                .WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<OrdenesServicio>(entity =>
        {
            entity.ToTable("OrdenesServicio");

            entity.HasKey(e => e.IdOrden);

            entity.Property(e => e.Costo).HasPrecision(10, 2);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Estado).HasMaxLength(30);

            entity.HasOne(d => d.IdVehiculoNavigation)
                .WithMany(p => p.OrdenesServicios)
                .HasForeignKey(d => d.IdVehiculo)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
