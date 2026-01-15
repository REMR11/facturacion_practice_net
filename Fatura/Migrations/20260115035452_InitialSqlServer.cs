using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fatura.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_categoria = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categoria", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nit_dui = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cliente", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "marca",
                columns: table => new
                {
                    id_marca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_marca = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_marca", x => x.id_marca);
                });

            migrationBuilder.CreateTable(
                name: "metodo_pago",
                columns: table => new
                {
                    id_metodo_pago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    requiere_referencia = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_metodo_pago", x => x.id_metodo_pago);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tipo_impuesto",
                columns: table => new
                {
                    id_tipo_impuesto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    porcentaje = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    aplica_productos = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    aplica_servicios = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipo_impuesto", x => x.id_tipo_impuesto);
                });

            migrationBuilder.CreateTable(
                name: "unidad_medida",
                columns: table => new
                {
                    id_unidad_medida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    abreviatura = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unidad_medida", x => x.id_unidad_medida);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_usuario = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    nombre_completo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    contraseña_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    salt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ultimo_acceso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuario", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    id_producto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo = table.Column<int>(type: "int", nullable: false),
                    id_marca = table.Column<int>(type: "int", nullable: true),
                    id_categoria = table.Column<int>(type: "int", nullable: true),
                    id_unidad_medida = table.Column<int>(type: "int", nullable: true),
                    nombre_producto = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    precio = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    codigo = table.Column<int>(type: "int", nullable: true),
                    stock = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    stock_minimo = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_producto", x => x.id_producto);
                    table.ForeignKey(
                        name: "fk_producto_categoria",
                        column: x => x.id_categoria,
                        principalTable: "categoria",
                        principalColumn: "id_categoria",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_producto_marca",
                        column: x => x.id_marca,
                        principalTable: "marca",
                        principalColumn: "id_marca",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_producto_unidad_medida",
                        column: x => x.id_unidad_medida,
                        principalTable: "unidad_medida",
                        principalColumn: "id_unidad_medida",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "configuracion_dashboard",
                columns: table => new
                {
                    id_configuracion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    mostrar_ingresos_mes = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    mostrar_facturas_emitidas = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    mostrar_clientes_activos = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    mostrar_tasa_cobro = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    mostrar_ingresos_mensuales = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    mostrar_facturas_recientes = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    periodo_grafico_meses = table.Column<int>(type: "int", nullable: false, defaultValue: 7),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configuracion_dashboard", x => x.id_configuracion);
                    table.ForeignKey(
                        name: "fk_configuracion_dashboard_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "factura",
                columns: table => new
                {
                    id_factura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numero_factura = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    serie_factura = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    cai_dte = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fecha_limite_emision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_vencimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    tipo_documento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Factura"),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    cliente_nit_dui = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    cliente_nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    cliente_direccion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    sub_total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    iva = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    isr = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    otros_impuestos = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    estado = table.Column<int>(type: "int", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    id_metodo_pago = table.Column<int>(type: "int", nullable: true),
                    referencia_metodo_pago = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fecha_pago = table.Column<DateTime>(type: "datetime2", nullable: true),
                    moneda_simbolo = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, defaultValue: "$"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_factura", x => x.id_factura);
                    table.ForeignKey(
                        name: "fk_factura_cliente",
                        column: x => x.cliente_id,
                        principalTable: "cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_factura_metodo_pago",
                        column: x => x.id_metodo_pago,
                        principalTable: "metodo_pago",
                        principalColumn: "id_metodo_pago",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_factura_usuario",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "usuario_role",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    asignado_en = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuario_role", x => new { x.usuario_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_usuario_role_role",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_usuario_role_usuario",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "producto_impuesto",
                columns: table => new
                {
                    id_producto = table.Column<int>(type: "int", nullable: false),
                    id_tipo_impuesto = table.Column<int>(type: "int", nullable: false),
                    porcentaje_personalizado = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_producto_impuesto", x => new { x.id_producto, x.id_tipo_impuesto });
                    table.ForeignKey(
                        name: "fk_producto_impuesto_producto",
                        column: x => x.id_producto,
                        principalTable: "producto",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_producto_impuesto_tipo_impuesto",
                        column: x => x.id_tipo_impuesto,
                        principalTable: "tipo_impuesto",
                        principalColumn: "id_tipo_impuesto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalle_factura",
                columns: table => new
                {
                    id_detalle_factura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_factura = table.Column<int>(type: "int", nullable: false),
                    id_producto = table.Column<int>(type: "int", nullable: true),
                    nombre_producto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    unidad_medida = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio_unitario = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    descuento = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_detalle_factura", x => x.id_detalle_factura);
                    table.ForeignKey(
                        name: "fk_detalle_factura_factura",
                        column: x => x.id_factura,
                        principalTable: "factura",
                        principalColumn: "id_factura",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_detalle_factura_producto",
                        column: x => x.id_producto,
                        principalTable: "producto",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "historial_transaccion",
                columns: table => new
                {
                    id_historial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_factura = table.Column<int>(type: "int", nullable: true),
                    id_usuario_emisor = table.Column<int>(type: "int", nullable: false),
                    id_cliente = table.Column<int>(type: "int", nullable: true),
                    tipo_transaccion = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    monto = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    fecha_transaccion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    datos_adicionales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_historial_transaccion", x => x.id_historial);
                    table.ForeignKey(
                        name: "fk_historial_transaccion_cliente",
                        column: x => x.id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_historial_transaccion_factura",
                        column: x => x.id_factura,
                        principalTable: "factura",
                        principalColumn: "id_factura",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_historial_transaccion_usuario",
                        column: x => x.id_usuario_emisor,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "detalle_factura_impuesto",
                columns: table => new
                {
                    id_detalle_factura_impuesto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_detalle_factura = table.Column<int>(type: "int", nullable: false),
                    id_tipo_impuesto = table.Column<int>(type: "int", nullable: false),
                    nombre_impuesto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    porcentaje = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    base_imponible = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    monto_impuesto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_detalle_factura_impuesto", x => x.id_detalle_factura_impuesto);
                    table.ForeignKey(
                        name: "fk_detalle_factura_impuesto_detalle_factura",
                        column: x => x.id_detalle_factura,
                        principalTable: "detalle_factura",
                        principalColumn: "id_detalle_factura",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_detalle_factura_impuesto_tipo_impuesto",
                        column: x => x.id_tipo_impuesto,
                        principalTable: "tipo_impuesto",
                        principalColumn: "id_tipo_impuesto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "idx_cliente_email",
                table: "cliente",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "uk_cliente_nit_dui",
                table: "cliente",
                column: "nit_dui",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_configuracion_dashboard_usuario",
                table: "configuracion_dashboard",
                column: "id_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_detalle_factura_id_factura",
                table: "detalle_factura",
                column: "id_factura");

            migrationBuilder.CreateIndex(
                name: "idx_detalle_factura_id_producto",
                table: "detalle_factura",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "idx_detalle_factura_impuesto_id_detalle_factura",
                table: "detalle_factura_impuesto",
                column: "id_detalle_factura");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_factura_impuesto_id_tipo_impuesto",
                table: "detalle_factura_impuesto",
                column: "id_tipo_impuesto");

            migrationBuilder.CreateIndex(
                name: "idx_factura_cliente_id",
                table: "factura",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "idx_factura_estado",
                table: "factura",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "idx_factura_fecha_creacion",
                table: "factura",
                column: "fecha_creacion");

            migrationBuilder.CreateIndex(
                name: "idx_factura_id_metodo_pago",
                table: "factura",
                column: "id_metodo_pago");

            migrationBuilder.CreateIndex(
                name: "idx_factura_usuario_id",
                table: "factura",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "uk_factura_numero_factura",
                table: "factura",
                column: "numero_factura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_historial_transaccion_fecha_transaccion",
                table: "historial_transaccion",
                column: "fecha_transaccion");

            migrationBuilder.CreateIndex(
                name: "idx_historial_transaccion_id_cliente",
                table: "historial_transaccion",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "idx_historial_transaccion_id_factura",
                table: "historial_transaccion",
                column: "id_factura");

            migrationBuilder.CreateIndex(
                name: "idx_historial_transaccion_id_usuario_emisor",
                table: "historial_transaccion",
                column: "id_usuario_emisor");

            migrationBuilder.CreateIndex(
                name: "idx_historial_transaccion_tipo_transaccion",
                table: "historial_transaccion",
                column: "tipo_transaccion");

            migrationBuilder.CreateIndex(
                name: "uk_metodo_pago_nombre",
                table: "metodo_pago",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_producto_id_categoria",
                table: "producto",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_producto_id_marca",
                table: "producto",
                column: "id_marca");

            migrationBuilder.CreateIndex(
                name: "IX_producto_id_unidad_medida",
                table: "producto",
                column: "id_unidad_medida");

            migrationBuilder.CreateIndex(
                name: "uk_producto_codigo",
                table: "producto",
                column: "codigo",
                unique: true,
                filter: "[codigo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_producto_impuesto_id_tipo_impuesto",
                table: "producto_impuesto",
                column: "id_tipo_impuesto");

            migrationBuilder.CreateIndex(
                name: "uk_role_nombre",
                table: "role",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_tipo_impuesto_nombre",
                table: "tipo_impuesto",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_unidad_medida_nombre",
                table: "unidad_medida",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_usuario_nombre_usuario",
                table: "usuario",
                column: "nombre_usuario");

            migrationBuilder.CreateIndex(
                name: "uk_usuario_email",
                table: "usuario",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_role_role_id",
                table: "usuario_role",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configuracion_dashboard");

            migrationBuilder.DropTable(
                name: "detalle_factura_impuesto");

            migrationBuilder.DropTable(
                name: "historial_transaccion");

            migrationBuilder.DropTable(
                name: "producto_impuesto");

            migrationBuilder.DropTable(
                name: "usuario_role");

            migrationBuilder.DropTable(
                name: "detalle_factura");

            migrationBuilder.DropTable(
                name: "tipo_impuesto");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "factura");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropTable(
                name: "metodo_pago");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "categoria");

            migrationBuilder.DropTable(
                name: "marca");

            migrationBuilder.DropTable(
                name: "unidad_medida");
        }
    }
}
