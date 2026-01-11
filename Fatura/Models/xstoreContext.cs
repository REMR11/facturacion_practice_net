using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fatura.Models
{
    public partial class xstoreContext : DbContext
    {


        public xstoreContext()
        {
        }

        public xstoreContext(DbContextOptions<xstoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categorium> Categoria { get; set; } = null!;
        public virtual DbSet<DetalleFactura> DetalleFacturas { get; set; } = null!;
        public virtual DbSet<Factura> Facturas { get; set; } = null!;
        public virtual DbSet<Marca> Marcas { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=RIQUELME;Database=xstore;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorium>(entity =>
            {
                entity.HasKey(e => e.Idcategoria)
                    .HasName("PK__Categori__70E82E28335B1CB4");

                entity.Property(e => e.Idcategoria).HasColumnName("IDCategoria");

                entity.Property(e => e.NombreCategoria)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DetalleFactura>(entity =>
            {
                entity.HasKey(e => e.IddetalleFactura)
                    .HasName("PK__DetalleF__EF0E5D9A0E87D534");

                entity.ToTable("DetalleFactura");

                entity.Property(e => e.IddetalleFactura).HasColumnName("IDDetalleFactura");

                entity.Property(e => e.FechaCompra).HasColumnType("datetime");

                entity.Property(e => e.Idfactura).HasColumnName("IDFactura");

                entity.Property(e => e.Idproducto).HasColumnName("IDProducto");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.HasOne(d => d.IdfacturaNavigation)
                    .WithMany(p => p.DetalleFacturas)
                    .HasForeignKey(d => d.Idfactura)
                    .HasConstraintName("FK__DetalleFa__IDFac__33D4B598");

                entity.HasOne(d => d.IdproductoNavigation)
                    .WithMany(p => p.DetalleFacturas)
                    .HasForeignKey(d => d.Idproducto)
                    .HasConstraintName("FK__DetalleFa__IDPro__34C8D9D1");
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasKey(e => e.Idfactura)
                    .HasName("PK__Factura__492FE9390232D7A0");

                entity.ToTable("Factura");

                entity.Property(e => e.Idfactura).HasColumnName("IDFactura");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.Total).HasColumnType("money");
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.Idmarca)
                    .HasName("PK__Marca__CEC375E729BA7C81");

                entity.ToTable("Marca");

                entity.Property(e => e.Idmarca).HasColumnName("IDMarca");

                entity.Property(e => e.NombreMarca)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Idproducto)
                    .HasName("PK__Producto__ABDAF2B48FF32BD9");

                entity.ToTable("Producto");

                entity.Property(e => e.Idproducto).HasColumnName("IDProducto");

                entity.Property(e => e.Codigo).HasColumnName("codigo");

                entity.Property(e => e.Idcategoria).HasColumnName("IDCategoria");

                entity.Property(e => e.Idmarca).HasColumnName("IDMarca");

                entity.Property(e => e.NombreProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Precio).HasColumnName("precio");

                entity.HasOne(d => d.IdcategoriaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.Idcategoria)
                    .HasConstraintName("FK__Producto__IDCate__2F10007B");

                entity.HasOne(d => d.IdmarcaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.Idmarca)
                    .HasConstraintName("FK__Producto__IDMarc__2E1BDC42");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Idusurio)
                    .HasName("PK__Usuario__60FD6F4FAA1CC3B0");

                entity.ToTable("Usuario");

                entity.Property(e => e.Idusurio).HasColumnName("IDUsurio");

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("NombreUSuario");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
