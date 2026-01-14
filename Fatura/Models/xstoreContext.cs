using Microsoft.EntityFrameworkCore;
using Fatura.Models.Core;
using Fatura.Models.Identity;
using Fatura.Models.Catalogos;
using Fatura.Models.Facturacion;
using Fatura.Models.Auditoria;
using Fatura.Models.Enums;

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

        public virtual DbSet<Catalogos.Categoria> Categoria { get; set; } = null!;
        public virtual DbSet<Facturacion.Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Identity.ConfiguracionDashboard> ConfiguracionDashboards { get; set; } = null!;
        public virtual DbSet<Facturacion.DetalleFactura> DetalleFacturas { get; set; } = null!;
        public virtual DbSet<Facturacion.DetalleFacturaImpuesto> DetalleFacturaImpuestos { get; set; } = null!;
        public virtual DbSet<Facturacion.Factura> Facturas { get; set; } = null!;
        public virtual DbSet<Auditoria.HistorialTransaccion> HistorialTransacciones { get; set; } = null!;
        public virtual DbSet<Catalogos.Marca> Marcas { get; set; } = null!;
        public virtual DbSet<Catalogos.MetodoPago> MetodoPagos { get; set; } = null!;
        public virtual DbSet<Catalogos.Producto> Productos { get; set; } = null!;
        public virtual DbSet<Catalogos.ProductoImpuesto> ProductoImpuestos { get; set; } = null!;
        public virtual DbSet<Identity.Role> Roles { get; set; } = null!;
        public virtual DbSet<Catalogos.TipoImpuesto> TipoImpuestos { get; set; } = null!;
        public virtual DbSet<Catalogos.UnidadMedida> UnidadMedidas { get; set; } = null!;
        public virtual DbSet<Identity.Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Identity.UsuarioRole> UsuarioRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // La configuración se realiza en Program.cs mediante DI
            // No es necesario configurar aquí si se usa AddDbContext
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Catalogos.Categoria>(entity =>
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
                    .IsRequired();
            });

            modelBuilder.Entity<Facturacion.DetalleFactura>(entity =>
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

                entity.Property(e => e.NombreProducto)
                    .HasColumnName("nombre_producto")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.UnidadMedida)
                    .HasColumnName("unidad_medida")
                    .HasMaxLength(20);

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

            modelBuilder.Entity<Facturacion.Factura>(entity =>
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
                    .HasColumnType("datetime2");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("fecha_creacion")
                    .HasColumnType("datetime2")
                    .IsRequired();

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnName("fecha_vencimiento")
                    .HasColumnType("datetime2");

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

                entity.Property(e => e.IdMetodoPago)
                    .HasColumnName("id_metodo_pago");

                entity.Property(e => e.ReferenciaMetodoPago)
                    .HasColumnName("referencia_metodo_pago")
                    .HasMaxLength(100);

                entity.Property(e => e.FechaPago)
                    .HasColumnName("fecha_pago")
                    .HasColumnType("datetime2");

                entity.Property(e => e.MonedaSimbolo)
                    .HasColumnName("moneda_simbolo")
                    .HasMaxLength(5)
                    .HasDefaultValue("$");

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

                entity.HasOne(d => d.MetodoPago)
                    .WithMany(p => p.Facturas)
                    .HasForeignKey(d => d.IdMetodoPago)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_factura_metodo_pago");

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
                entity.HasIndex(e => e.IdMetodoPago)
                    .HasDatabaseName("idx_factura_id_metodo_pago");
            });

            modelBuilder.Entity<Catalogos.MetodoPago>(entity =>
            {
                entity.ToTable("metodo_pago");

                entity.HasKey(e => e.IdMetodoPago)
                    .HasName("pk_metodo_pago");

                entity.Property(e => e.IdMetodoPago)
                    .HasColumnName("id_metodo_pago")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(200);

                entity.Property(e => e.RequiereReferencia)
                    .HasColumnName("requiere_referencia")
                    .HasDefaultValue(false);

                entity.Property(e => e.Activo)
                    .HasColumnName("activo")
                    .HasDefaultValue(true);

                entity.HasIndex(e => e.Nombre)
                    .IsUnique()
                    .HasDatabaseName("uk_metodo_pago_nombre");
            });

            modelBuilder.Entity<Catalogos.UnidadMedida>(entity =>
            {
                entity.ToTable("unidad_medida");

                entity.HasKey(e => e.IdUnidadMedida)
                    .HasName("pk_unidad_medida");

                entity.Property(e => e.IdUnidadMedida)
                    .HasColumnName("id_unidad_medida")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Abreviatura)
                    .HasColumnName("abreviatura")
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(200);

                entity.Property(e => e.Activo)
                    .HasColumnName("activo")
                    .HasDefaultValue(true);

                entity.HasIndex(e => e.Nombre)
                    .IsUnique()
                    .HasDatabaseName("uk_unidad_medida_nombre");
            });

            modelBuilder.Entity<Catalogos.TipoImpuesto>(entity =>
            {
                entity.ToTable("tipo_impuesto");

                entity.HasKey(e => e.IdTipoImpuesto)
                    .HasName("pk_tipo_impuesto");

                entity.Property(e => e.IdTipoImpuesto)
                    .HasColumnName("id_tipo_impuesto")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(200);

                entity.Property(e => e.Porcentaje)
                    .HasColumnName("porcentaje")
                    .HasColumnType("numeric(5,2)")
                    .IsRequired();

                entity.Property(e => e.AplicaProductos)
                    .HasColumnName("aplica_productos")
                    .HasDefaultValue(true);

                entity.Property(e => e.AplicaServicios)
                    .HasColumnName("aplica_servicios")
                    .HasDefaultValue(true);

                entity.Property(e => e.Activo)
                    .HasColumnName("activo")
                    .HasDefaultValue(true);

                entity.HasIndex(e => e.Nombre)
                    .IsUnique()
                    .HasDatabaseName("uk_tipo_impuesto_nombre");
            });

            modelBuilder.Entity<Catalogos.ProductoImpuesto>(entity =>
            {
                entity.ToTable("producto_impuesto");

                entity.HasKey(e => new { e.IdProducto, e.IdTipoImpuesto })
                    .HasName("pk_producto_impuesto");

                entity.Property(e => e.IdProducto)
                    .HasColumnName("id_producto");

                entity.Property(e => e.IdTipoImpuesto)
                    .HasColumnName("id_tipo_impuesto");

                entity.Property(e => e.PorcentajePersonalizado)
                    .HasColumnName("porcentaje_personalizado")
                    .HasColumnType("numeric(5,2)");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.ProductoImpuestos)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_producto_impuesto_producto");

                entity.HasOne(d => d.TipoImpuesto)
                    .WithMany(p => p.ProductoImpuestos)
                    .HasForeignKey(d => d.IdTipoImpuesto)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_producto_impuesto_tipo_impuesto");
            });

            modelBuilder.Entity<Facturacion.DetalleFacturaImpuesto>(entity =>
            {
                entity.ToTable("detalle_factura_impuesto");

                entity.HasKey(e => e.IdDetalleFacturaImpuesto)
                    .HasName("pk_detalle_factura_impuesto");

                entity.Property(e => e.IdDetalleFacturaImpuesto)
                    .HasColumnName("id_detalle_factura_impuesto")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdDetalleFactura)
                    .HasColumnName("id_detalle_factura")
                    .IsRequired();

                entity.Property(e => e.IdTipoImpuesto)
                    .HasColumnName("id_tipo_impuesto")
                    .IsRequired();

                entity.Property(e => e.NombreImpuesto)
                    .HasColumnName("nombre_impuesto")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Porcentaje)
                    .HasColumnName("porcentaje")
                    .HasColumnType("numeric(5,2)")
                    .IsRequired();

                entity.Property(e => e.BaseImponible)
                    .HasColumnName("base_imponible")
                    .HasColumnType("numeric(18,2)")
                    .IsRequired();

                entity.Property(e => e.MontoImpuesto)
                    .HasColumnName("monto_impuesto")
                    .HasColumnType("numeric(18,2)")
                    .IsRequired();

                entity.HasOne(d => d.DetalleFactura)
                    .WithMany(p => p.Impuestos)
                    .HasForeignKey(d => d.IdDetalleFactura)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_detalle_factura_impuesto_detalle_factura");

                entity.HasOne(d => d.TipoImpuesto)
                    .WithMany(p => p.DetalleFacturaImpuestos)
                    .HasForeignKey(d => d.IdTipoImpuesto)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_detalle_factura_impuesto_tipo_impuesto");

                entity.HasIndex(e => e.IdDetalleFactura)
                    .HasDatabaseName("idx_detalle_factura_impuesto_id_detalle_factura");
            });

            modelBuilder.Entity<Auditoria.HistorialTransaccion>(entity =>
            {
                entity.ToTable("historial_transaccion");

                entity.HasKey(e => e.IdHistorial)
                    .HasName("pk_historial_transaccion");

                entity.Property(e => e.IdHistorial)
                    .HasColumnName("id_historial")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdFactura)
                    .HasColumnName("id_factura");

                entity.Property(e => e.IdUsuarioEmisor)
                    .HasColumnName("id_usuario_emisor")
                    .IsRequired();

                entity.Property(e => e.IdCliente)
                    .HasColumnName("id_cliente");

                entity.Property(e => e.TipoTransaccion)
                    .HasColumnName("tipo_transaccion")
                    .HasConversion<int>()
                    .IsRequired();

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.Monto)
                    .HasColumnName("monto")
                    .HasColumnType("numeric(18,2)");

                entity.Property(e => e.FechaTransaccion)
                    .HasColumnName("fecha_transaccion")
                    .HasColumnType("datetime2")
                    .IsRequired();

                entity.Property(e => e.DatosAdicionales)
                    .HasColumnName("datos_adicionales")
                    .HasColumnType("nvarchar(max)");

                entity.HasOne(d => d.Factura)
                    .WithMany(p => p.HistorialTransacciones)
                    .HasForeignKey(d => d.IdFactura)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_historial_transaccion_factura");

                entity.HasOne(d => d.UsuarioEmisor)
                    .WithMany(p => p.HistorialTransacciones)
                    .HasForeignKey(d => d.IdUsuarioEmisor)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_historial_transaccion_usuario");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.HistorialTransacciones)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_historial_transaccion_cliente");

                entity.HasIndex(e => e.IdFactura)
                    .HasDatabaseName("idx_historial_transaccion_id_factura");
                entity.HasIndex(e => e.IdUsuarioEmisor)
                    .HasDatabaseName("idx_historial_transaccion_id_usuario_emisor");
                entity.HasIndex(e => e.IdCliente)
                    .HasDatabaseName("idx_historial_transaccion_id_cliente");
                entity.HasIndex(e => e.FechaTransaccion)
                    .HasDatabaseName("idx_historial_transaccion_fecha_transaccion");
                entity.HasIndex(e => e.TipoTransaccion)
                    .HasDatabaseName("idx_historial_transaccion_tipo_transaccion");
            });

            modelBuilder.Entity<Catalogos.Marca>(entity =>
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
                    .IsRequired();
            });

            modelBuilder.Entity<Catalogos.Producto>(entity =>
            {
                entity.ToTable("producto");

                entity.HasKey(e => e.IdProducto)
                    .HasName("pk_producto");

                entity.Property(e => e.IdProducto)
                    .HasColumnName("id_producto")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Tipo)
                    .HasColumnName("tipo")
                    .HasConversion<int>()
                    .IsRequired();

                entity.Property(e => e.Codigo)
                    .HasColumnName("codigo");

                entity.Property(e => e.IdCategoria)
                    .HasColumnName("id_categoria");

                entity.Property(e => e.IdMarca)
                    .HasColumnName("id_marca");

                entity.Property(e => e.IdUnidadMedida)
                    .HasColumnName("id_unidad_medida");

                entity.Property(e => e.NombreProducto)
                    .HasColumnName("nombre_producto")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(500);

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

                entity.HasOne(d => d.UnidadMedida)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdUnidadMedida)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_producto_unidad_medida");
                
                entity.HasIndex(e => e.Codigo)
                    .IsUnique()
                    .HasDatabaseName("uk_producto_codigo");
            });

            modelBuilder.Entity<Identity.Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.HasKey(e => e.IdUsuario)
                    .HasName("pk_usuario");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.NombreUsuario)
                    .HasColumnName("nombre_usuario")
                    .HasMaxLength(40);

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

                entity.Property(e => e.NombreCompleto)
                    .HasColumnName("nombre_completo")
                    .HasMaxLength(100);

                entity.Property(e => e.UltimoAcceso)
                    .HasColumnName("ultimo_acceso")
                    .HasColumnType("datetime2");

                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("uk_usuario_email");
                entity.HasIndex(e => e.NombreUsuario)
                    .HasDatabaseName("idx_usuario_nombre_usuario");
            });

            modelBuilder.Entity<Identity.ConfiguracionDashboard>(entity =>
            {
                entity.ToTable("configuracion_dashboard");

                entity.HasKey(e => e.IdConfiguracion)
                    .HasName("pk_configuracion_dashboard");

                entity.Property(e => e.IdConfiguracion)
                    .HasColumnName("id_configuracion")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario")
                    .IsRequired();

                entity.Property(e => e.MostrarIngresosMes)
                    .HasColumnName("mostrar_ingresos_mes")
                    .HasDefaultValue(true);

                entity.Property(e => e.MostrarFacturasEmitidas)
                    .HasColumnName("mostrar_facturas_emitidas")
                    .HasDefaultValue(true);

                entity.Property(e => e.MostrarClientesActivos)
                    .HasColumnName("mostrar_clientes_activos")
                    .HasDefaultValue(true);

                entity.Property(e => e.MostrarTasaCobro)
                    .HasColumnName("mostrar_tasa_cobro")
                    .HasDefaultValue(true);

                entity.Property(e => e.MostrarIngresosMensuales)
                    .HasColumnName("mostrar_ingresos_mensuales")
                    .HasDefaultValue(true);

                entity.Property(e => e.MostrarFacturasRecientes)
                    .HasColumnName("mostrar_facturas_recientes")
                    .HasDefaultValue(true);

                entity.Property(e => e.PeriodoGraficoMeses)
                    .HasColumnName("periodo_grafico_meses")
                    .HasDefaultValue(7);

                // Configuración de relación uno-a-uno: ConfiguracionDashboard es la entidad dependiente
                entity.HasOne(d => d.Usuario)
                    .WithOne(p => p.ConfiguracionDashboard)
                    .HasForeignKey<ConfiguracionDashboard>(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_configuracion_dashboard_usuario");

                entity.HasIndex(e => e.IdUsuario)
                    .IsUnique()
                    .HasDatabaseName("uk_configuracion_dashboard_usuario");
            });

            modelBuilder.Entity<Facturacion.Cliente>(entity =>
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

            modelBuilder.Entity<Identity.Role>(entity =>
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

            modelBuilder.Entity<Identity.UsuarioRole>(entity =>
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
