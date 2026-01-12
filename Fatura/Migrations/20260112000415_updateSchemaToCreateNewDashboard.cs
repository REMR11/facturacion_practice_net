using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fatura.Migrations
{
    /// <inheritdoc />
    public partial class updateSchemaToCreateNewDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nombre_completo",
                table: "usuario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "descripcion",
                table: "producto",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_unidad_medida",
                table: "producto",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tipo",
                table: "producto",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_pago",
                table: "factura",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_metodo_pago",
                table: "factura",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "moneda_simbolo",
                table: "factura",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "$");

            migrationBuilder.AddColumn<string>(
                name: "referencia_metodo_pago",
                table: "factura",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nombre_producto",
                table: "detalle_factura",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "unidad_medida",
                table: "detalle_factura",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "configuracion_dashboard",
                columns: table => new
                {
                    id_configuracion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    mostrar_ingresos_mes = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    mostrar_facturas_emitidas = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    mostrar_clientes_activos = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    mostrar_tasa_cobro = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    mostrar_ingresos_mensuales = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    mostrar_facturas_recientes = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    periodo_grafico_meses = table.Column<int>(type: "integer", nullable: false, defaultValue: 7),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<int>(type: "integer", nullable: true)
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
                name: "historial_transaccion",
                columns: table => new
                {
                    id_historial = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_factura = table.Column<int>(type: "integer", nullable: true),
                    id_usuario_emisor = table.Column<int>(type: "integer", nullable: false),
                    id_cliente = table.Column<int>(type: "integer", nullable: true),
                    tipo_transaccion = table.Column<int>(type: "integer", nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    monto = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    fecha_transaccion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    datos_adicionales = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<int>(type: "integer", nullable: true)
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
                name: "metodo_pago",
                columns: table => new
                {
                    id_metodo_pago = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    requiere_referencia = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_metodo_pago", x => x.id_metodo_pago);
                });

            migrationBuilder.CreateTable(
                name: "tipo_impuesto",
                columns: table => new
                {
                    id_tipo_impuesto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    porcentaje = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    aplica_productos = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    aplica_servicios = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipo_impuesto", x => x.id_tipo_impuesto);
                });

            migrationBuilder.CreateTable(
                name: "unidad_medida",
                columns: table => new
                {
                    id_unidad_medida = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    abreviatura = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unidad_medida", x => x.id_unidad_medida);
                });

            migrationBuilder.CreateTable(
                name: "detalle_factura_impuesto",
                columns: table => new
                {
                    id_detalle_factura_impuesto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_detalle_factura = table.Column<int>(type: "integer", nullable: false),
                    id_tipo_impuesto = table.Column<int>(type: "integer", nullable: false),
                    nombre_impuesto = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    porcentaje = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    base_imponible = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    monto_impuesto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "producto_impuesto",
                columns: table => new
                {
                    id_producto = table.Column<int>(type: "integer", nullable: false),
                    id_tipo_impuesto = table.Column<int>(type: "integer", nullable: false),
                    porcentaje_personalizado = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_producto_id_unidad_medida",
                table: "producto",
                column: "id_unidad_medida");

            migrationBuilder.CreateIndex(
                name: "idx_factura_id_metodo_pago",
                table: "factura",
                column: "id_metodo_pago");

            migrationBuilder.CreateIndex(
                name: "uk_configuracion_dashboard_usuario",
                table: "configuracion_dashboard",
                column: "id_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_detalle_factura_impuesto_id_detalle_factura",
                table: "detalle_factura_impuesto",
                column: "id_detalle_factura");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_factura_impuesto_id_tipo_impuesto",
                table: "detalle_factura_impuesto",
                column: "id_tipo_impuesto");

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
                name: "IX_producto_impuesto_id_tipo_impuesto",
                table: "producto_impuesto",
                column: "id_tipo_impuesto");

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

            migrationBuilder.AddForeignKey(
                name: "fk_factura_metodo_pago",
                table: "factura",
                column: "id_metodo_pago",
                principalTable: "metodo_pago",
                principalColumn: "id_metodo_pago",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_producto_unidad_medida",
                table: "producto",
                column: "id_unidad_medida",
                principalTable: "unidad_medida",
                principalColumn: "id_unidad_medida",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_factura_metodo_pago",
                table: "factura");

            migrationBuilder.DropForeignKey(
                name: "fk_producto_unidad_medida",
                table: "producto");

            migrationBuilder.DropTable(
                name: "configuracion_dashboard");

            migrationBuilder.DropTable(
                name: "detalle_factura_impuesto");

            migrationBuilder.DropTable(
                name: "historial_transaccion");

            migrationBuilder.DropTable(
                name: "metodo_pago");

            migrationBuilder.DropTable(
                name: "producto_impuesto");

            migrationBuilder.DropTable(
                name: "unidad_medida");

            migrationBuilder.DropTable(
                name: "tipo_impuesto");

            migrationBuilder.DropIndex(
                name: "IX_producto_id_unidad_medida",
                table: "producto");

            migrationBuilder.DropIndex(
                name: "idx_factura_id_metodo_pago",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "nombre_completo",
                table: "usuario");

            migrationBuilder.DropColumn(
                name: "descripcion",
                table: "producto");

            migrationBuilder.DropColumn(
                name: "id_unidad_medida",
                table: "producto");

            migrationBuilder.DropColumn(
                name: "tipo",
                table: "producto");

            migrationBuilder.DropColumn(
                name: "fecha_pago",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "id_metodo_pago",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "moneda_simbolo",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "referencia_metodo_pago",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "nombre_producto",
                table: "detalle_factura");

            migrationBuilder.DropColumn(
                name: "unidad_medida",
                table: "detalle_factura");
        }
    }
}
