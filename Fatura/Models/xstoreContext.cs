using Microsoft.EntityFrameworkCore;

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

        public virtual DbSet<Categoria> Categoria { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<DetalleFactura> DetalleFacturas { get; set; } = null!;
        public virtual DbSet<Factura> Facturas { get; set; } = null!;
        public virtual DbSet<Marca> Marcas { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<UsuarioRole> UsuarioRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // La configuración se realiza en Program.cs mediante DI
            // No es necesario configurar aquí si se usa AddDbContext
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("categoria");

                entity.HasKey(e => e.IdCategoria)
                    .HasName("pk_categoria");

                entity.Property(e => e.IdCategoria)
                    .HasColumnName("id_categoria")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NombreCategoria)
                    .HasColumnName("nombre_categoria")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();
            });

            modelBuilder.Entity<DetalleFactura>(entity =>
            {
                entity.ToTable("detalle_factura");

                entity.HasKey(e => e.IdDetalleFactura)
                    .HasName("pk_detalle_factura");

                entity.Property(e => e.IdDetalleFactura)
                    .HasColumnName("id_detalle_factura")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdFactura)
                    .HasColumnName("id_factura")
                    .IsRequired();

                entity.Property(e => e.IdProducto)
                    .HasColumnName("id_producto");

                entity.Property(e => e.Cantidad)
                    .HasColumnName("cantidad")
                    .IsRequired();

                entity.Property(e => e.PrecioUnitario)
                    .HasColumnName("precio_unitario")
                    .HasColumnType("numeric(18,2)")
                    .IsRequired();

                entity.Property(e => e.Descuento)
                    .HasColumnName("descuento")
                    .HasColumnType("numeric(18,2)")
                    .HasDefaultValue(0);

                entity.Ignore(e => e.Total); // Propiedad calculada, no se almacena

                entity.HasOne(d => d.Factura)
                    .WithMany(p => p.DetalleFacturas)
                    .HasForeignKey(d => d.IdFactura)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_detalle_factura_factura");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.DetalleFacturas)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_detalle_factura_producto");
                
                entity.HasIndex(e => e.IdFactura)
                    .HasDatabaseName("idx_detalle_factura_id_factura");
                entity.HasIndex(e => e.IdProducto)
                    .HasDatabaseName("idx_detalle_factura_id_producto");
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.ToTable("factura");

                entity.HasKey(e => e.IdFactura)
                    .HasName("pk_factura");

                entity.Property(e => e.IdFactura)
                    .HasColumnName("id_factura")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NumeroFactura)
                    .HasColumnName("numero_factura")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.SerieFactura)
                    .HasColumnName("serie_factura")
                    .HasMaxLength(20);

                entity.Property(e => e.CaiDte)
                    .HasColumnName("cai_dte")
                    .HasMaxLength(50);

                entity.Property(e => e.FechaLimiteEmision)
                    .HasColumnName("fecha_limite_emision")
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("fecha_creacion")
                    .HasColumnType("timestamp without time zone")
                    .IsRequired();

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnName("fecha_vencimiento")
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.TipoDocumento)
                    .HasColumnName("tipo_documento")
                    .HasMaxLength(50)
                    .HasDefaultValue("Factura");

                entity.Property(e => e.ClienteId)
                    .HasColumnName("cliente_id")
                    .IsRequired();

                entity.Property(e => e.ClienteNitDui)
                    .HasColumnName("cliente_nit_dui")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.ClienteNombre)
                    .HasColumnName("cliente_nombre")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.ClienteDireccion)
                    .HasColumnName("cliente_direccion")
                    .HasMaxLength(500);

                entity.Property(e => e.SubTotal)
                    .HasColumnName("sub_total")
                    .HasColumnType("numeric(18,2)")
                    .IsRequired();

                entity.Property(e => e.Iva)
                    .HasColumnName("iva")
                    .HasColumnType("numeric(18,2)")
                    .IsRequired();

                entity.Property(e => e.Isr)
                    .HasColumnName("isr")
                    .HasColumnType("numeric(18,2)")
                    .IsRequired();

                entity.Property(e => e.OtrosImpuestos)
                    .HasColumnName("otros_impuestos")
                    .HasColumnType("numeric(18,2)")
                    .IsRequired();

                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasColumnType("numeric(18,2)")
                    .IsRequired();

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasConversion<int>()
                    .IsRequired();

                entity.Property(e => e.UsuarioId)
                    .HasColumnName("usuario_id")
                    .IsRequired();

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Facturas)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_factura_cliente");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Facturas)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_factura_usuario");

                entity.HasIndex(e => e.NumeroFactura)
                    .IsUnique()
                    .HasDatabaseName("uk_factura_numero_factura");
                entity.HasIndex(e => e.FechaCreacion)
                    .HasDatabaseName("idx_factura_fecha_creacion");
                entity.HasIndex(e => e.UsuarioId)
                    .HasDatabaseName("idx_factura_usuario_id");
                entity.HasIndex(e => e.ClienteId)
                    .HasDatabaseName("idx_factura_cliente_id");
                entity.HasIndex(e => e.Estado)
                    .HasDatabaseName("idx_factura_estado");
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.ToTable("marca");

                entity.HasKey(e => e.IdMarca)
                    .HasName("pk_marca");

                entity.Property(e => e.IdMarca)
                    .HasColumnName("id_marca")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NombreMarca)
                    .HasColumnName("nombre_marca")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("producto");

                entity.HasKey(e => e.IdProducto)
                    .HasName("pk_producto");

                entity.Property(e => e.IdProducto)
                    .HasColumnName("id_producto")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Codigo)
                    .HasColumnName("codigo");

                entity.Property(e => e.IdCategoria)
                    .HasColumnName("id_categoria");

                entity.Property(e => e.IdMarca)
                    .HasColumnName("id_marca");

                entity.Property(e => e.NombreProducto)
                    .HasColumnName("nombre_producto")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.Precio)
                    .HasColumnName("precio")
                    .HasColumnType("numeric(18,2)");

                entity.Property(e => e.Stock)
                    .HasColumnName("stock")
                    .HasDefaultValue(0);

                entity.Property(e => e.StockMinimo)
                    .HasColumnName("stock_minimo")
                    .HasDefaultValue(0);

                entity.Property(e => e.Activo)
                    .HasColumnName("activo")
                    .HasDefaultValue(true);

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_producto_categoria");

                entity.HasOne(d => d.Marca)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdMarca)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_producto_marca");
                
                entity.HasIndex(e => e.Codigo)
                    .IsUnique()
                    .HasDatabaseName("uk_producto_codigo");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.HasKey(e => e.IdUsuario)
                    .HasName("pk_usuario");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NombreUsuario)
                    .HasColumnName("nombre_usuario")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.ContraseñaHash)
                    .HasColumnName("contraseña_hash")
                    .HasMaxLength(255);

                entity.Property(e => e.Salt)
                    .HasColumnName("salt")
                    .HasMaxLength(50);

                entity.Property(e => e.Activo)
                    .HasColumnName("activo")
                    .HasDefaultValue(true);

                entity.Property(e => e.UltimoAcceso)
                    .HasColumnName("ultimo_acceso")
                    .HasColumnType("timestamp without time zone");

                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("uk_usuario_email");
                entity.HasIndex(e => e.NombreUsuario)
                    .HasDatabaseName("idx_usuario_nombre_usuario");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("cliente");

                entity.HasKey(e => e.Id)
                    .HasName("pk_cliente");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NitDui)
                    .HasColumnName("nit_dui")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasMaxLength(500);

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.Activo)
                    .HasColumnName("activo")
                    .HasDefaultValue(true);

                entity.HasIndex(e => e.NitDui)
                    .IsUnique()
                    .HasDatabaseName("uk_cliente_nit_dui");
                entity.HasIndex(e => e.Email)
                    .HasDatabaseName("idx_cliente_email");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.HasKey(e => e.Id)
                    .HasName("pk_role");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(200);

                entity.HasIndex(e => e.Nombre)
                    .IsUnique()
                    .HasDatabaseName("uk_role_nombre");
            });

            modelBuilder.Entity<UsuarioRole>(entity =>
            {
                entity.ToTable("usuario_role");

                entity.HasKey(e => new { e.UsuarioId, e.RoleId })
                    .HasName("pk_usuario_role");

                entity.Property(e => e.UsuarioId)
                    .HasColumnName("usuario_id");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id");

                entity.Property(e => e.AsignadoEn)
                    .HasColumnName("asignado_en")
                    .HasColumnType("timestamp without time zone")
                    .IsRequired();

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.UsuarioRoles)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_usuario_role_usuario");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UsuarioRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_usuario_role_role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
