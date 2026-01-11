using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fatura.Migrations
{
    /// <inheritdoc />
    public partial class createstandardtocasecreatednewclienttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__DetalleFa__IDFac__33D4B598",
                table: "DetalleFactura");

            migrationBuilder.DropForeignKey(
                name: "FK__DetalleFa__IDPro__34C8D9D1",
                table: "DetalleFactura");

            migrationBuilder.DropForeignKey(
                name: "FK_Factura_Usuario",
                table: "Factura");

            migrationBuilder.DropForeignKey(
                name: "FK__Producto__IDCate__2F10007B",
                table: "Producto");

            migrationBuilder.DropForeignKey(
                name: "FK__Producto__IDMarc__2E1BDC42",
                table: "Producto");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRole_Role",
                table: "UsuarioRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRole_Usuario",
                table: "UsuarioRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Usuario__60FD6F4FAA1CC3B0",
                table: "Usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Producto__ABDAF2B48FF32BD9",
                table: "Producto");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Marca__CEC375E729BA7C81",
                table: "Marca");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Factura__492FE9390232D7A0",
                table: "Factura");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Categori__70E82E28335B1CB4",
                table: "Categoria");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioRole",
                table: "UsuarioRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DetalleF__EF0E5D9A0E87D534",
                table: "DetalleFactura");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "usuario");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "role");

            migrationBuilder.RenameTable(
                name: "Producto",
                newName: "producto");

            migrationBuilder.RenameTable(
                name: "Marca",
                newName: "marca");

            migrationBuilder.RenameTable(
                name: "Factura",
                newName: "factura");

            migrationBuilder.RenameTable(
                name: "Categoria",
                newName: "categoria");

            migrationBuilder.RenameTable(
                name: "UsuarioRole",
                newName: "usuario_role");

            migrationBuilder.RenameTable(
                name: "DetalleFactura",
                newName: "detalle_factura");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "usuario",
                newName: "salt");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "usuario",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Activo",
                table: "usuario",
                newName: "activo");

            migrationBuilder.RenameColumn(
                name: "UltimoAcceso",
                table: "usuario",
                newName: "ultimo_acceso");

            migrationBuilder.RenameColumn(
                name: "NombreUsuario",
                table: "usuario",
                newName: "nombre_usuario");

            migrationBuilder.RenameColumn(
                name: "ContraseñaHash",
                table: "usuario",
                newName: "contraseña_hash");

            migrationBuilder.RenameColumn(
                name: "IDUsuario",
                table: "usuario",
                newName: "id_usuario");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_NombreUsuario",
                table: "usuario",
                newName: "idx_usuario_nombre_usuario");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_Email",
                table: "usuario",
                newName: "uk_usuario_email");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "role",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "role",
                newName: "descripcion");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "role",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Role_Nombre",
                table: "role",
                newName: "uk_role_nombre");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "producto",
                newName: "stock");

            migrationBuilder.RenameColumn(
                name: "Activo",
                table: "producto",
                newName: "activo");

            migrationBuilder.RenameColumn(
                name: "StockMinimo",
                table: "producto",
                newName: "stock_minimo");

            migrationBuilder.RenameColumn(
                name: "NombreProducto",
                table: "producto",
                newName: "nombre_producto");

            migrationBuilder.RenameColumn(
                name: "IDMarca",
                table: "producto",
                newName: "id_marca");

            migrationBuilder.RenameColumn(
                name: "IDCategoria",
                table: "producto",
                newName: "id_categoria");

            migrationBuilder.RenameColumn(
                name: "IDProducto",
                table: "producto",
                newName: "id_producto");

            migrationBuilder.RenameIndex(
                name: "IX_Producto_IDMarca",
                table: "producto",
                newName: "IX_producto_id_marca");

            migrationBuilder.RenameIndex(
                name: "IX_Producto_IDCategoria",
                table: "producto",
                newName: "IX_producto_id_categoria");

            migrationBuilder.RenameIndex(
                name: "IX_Producto_codigo",
                table: "producto",
                newName: "uk_producto_codigo");

            migrationBuilder.RenameColumn(
                name: "NombreMarca",
                table: "marca",
                newName: "nombre_marca");

            migrationBuilder.RenameColumn(
                name: "IDMarca",
                table: "marca",
                newName: "id_marca");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "factura",
                newName: "total");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "factura",
                newName: "estado");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "factura",
                newName: "usuario_id");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "factura",
                newName: "sub_total");

            migrationBuilder.RenameColumn(
                name: "NumeroFactura",
                table: "factura",
                newName: "numero_factura");

            migrationBuilder.RenameColumn(
                name: "FechaVencimiento",
                table: "factura",
                newName: "fecha_vencimiento");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "factura",
                newName: "fecha_creacion");

            migrationBuilder.RenameColumn(
                name: "IDFactura",
                table: "factura",
                newName: "id_factura");

            migrationBuilder.RenameColumn(
                name: "Impuesto",
                table: "factura",
                newName: "otros_impuestos");

            migrationBuilder.RenameIndex(
                name: "IX_Factura_UsuarioId",
                table: "factura",
                newName: "idx_factura_usuario_id");

            migrationBuilder.RenameIndex(
                name: "IX_Factura_NumeroFactura",
                table: "factura",
                newName: "uk_factura_numero_factura");

            migrationBuilder.RenameIndex(
                name: "IX_Factura_FechaCreacion",
                table: "factura",
                newName: "idx_factura_fecha_creacion");

            migrationBuilder.RenameIndex(
                name: "IX_Factura_Estado",
                table: "factura",
                newName: "idx_factura_estado");

            migrationBuilder.RenameColumn(
                name: "NombreCategoria",
                table: "categoria",
                newName: "nombre_categoria");

            migrationBuilder.RenameColumn(
                name: "IDCategoria",
                table: "categoria",
                newName: "id_categoria");

            migrationBuilder.RenameColumn(
                name: "AsignadoEn",
                table: "usuario_role",
                newName: "asignado_en");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "usuario_role",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "usuario_role",
                newName: "usuario_id");

            migrationBuilder.RenameIndex(
                name: "IX_UsuarioRole_RoleId",
                table: "usuario_role",
                newName: "IX_usuario_role_role_id");

            migrationBuilder.RenameColumn(
                name: "Descuento",
                table: "detalle_factura",
                newName: "descuento");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "detalle_factura",
                newName: "cantidad");

            migrationBuilder.RenameColumn(
                name: "PrecioUnitario",
                table: "detalle_factura",
                newName: "precio_unitario");

            migrationBuilder.RenameColumn(
                name: "IDProducto",
                table: "detalle_factura",
                newName: "id_producto");

            migrationBuilder.RenameColumn(
                name: "IDFactura",
                table: "detalle_factura",
                newName: "id_factura");

            migrationBuilder.RenameColumn(
                name: "IDDetalleFactura",
                table: "detalle_factura",
                newName: "id_detalle_factura");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleFactura_IDProducto",
                table: "detalle_factura",
                newName: "idx_detalle_factura_id_producto");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleFactura_IDFactura",
                table: "detalle_factura",
                newName: "idx_detalle_factura_id_factura");

            migrationBuilder.AddColumn<string>(
                name: "cai_dte",
                table: "factura",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cliente_direccion",
                table: "factura",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "cliente_id",
                table: "factura",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "cliente_nit_dui",
                table: "factura",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cliente_nombre",
                table: "factura",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_limite_emision",
                table: "factura",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "isr",
                table: "factura",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "iva",
                table: "factura",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "serie_factura",
                table: "factura",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo_documento",
                table: "factura",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Factura");

            migrationBuilder.AlterColumn<int>(
                name: "id_factura",
                table: "detalle_factura",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_usuario",
                table: "usuario",
                column: "id_usuario");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role",
                table: "role",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_producto",
                table: "producto",
                column: "id_producto");

            migrationBuilder.AddPrimaryKey(
                name: "pk_marca",
                table: "marca",
                column: "id_marca");

            migrationBuilder.AddPrimaryKey(
                name: "pk_factura",
                table: "factura",
                column: "id_factura");

            migrationBuilder.AddPrimaryKey(
                name: "pk_categoria",
                table: "categoria",
                column: "id_categoria");

            migrationBuilder.AddPrimaryKey(
                name: "pk_usuario_role",
                table: "usuario_role",
                columns: new[] { "usuario_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_detalle_factura",
                table: "detalle_factura",
                column: "id_detalle_factura");

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nit_dui = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("pk_cliente", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_factura_cliente_id",
                table: "factura",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "idx_cliente_email",
                table: "cliente",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "uk_cliente_nit_dui",
                table: "cliente",
                column: "nit_dui",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_detalle_factura_factura",
                table: "detalle_factura",
                column: "id_factura",
                principalTable: "factura",
                principalColumn: "id_factura",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_detalle_factura_producto",
                table: "detalle_factura",
                column: "id_producto",
                principalTable: "producto",
                principalColumn: "id_producto",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_factura_cliente",
                table: "factura",
                column: "cliente_id",
                principalTable: "cliente",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_factura_usuario",
                table: "factura",
                column: "usuario_id",
                principalTable: "usuario",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_producto_categoria",
                table: "producto",
                column: "id_categoria",
                principalTable: "categoria",
                principalColumn: "id_categoria",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_producto_marca",
                table: "producto",
                column: "id_marca",
                principalTable: "marca",
                principalColumn: "id_marca",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_usuario_role_role",
                table: "usuario_role",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_usuario_role_usuario",
                table: "usuario_role",
                column: "usuario_id",
                principalTable: "usuario",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_detalle_factura_factura",
                table: "detalle_factura");

            migrationBuilder.DropForeignKey(
                name: "fk_detalle_factura_producto",
                table: "detalle_factura");

            migrationBuilder.DropForeignKey(
                name: "fk_factura_cliente",
                table: "factura");

            migrationBuilder.DropForeignKey(
                name: "fk_factura_usuario",
                table: "factura");

            migrationBuilder.DropForeignKey(
                name: "fk_producto_categoria",
                table: "producto");

            migrationBuilder.DropForeignKey(
                name: "fk_producto_marca",
                table: "producto");

            migrationBuilder.DropForeignKey(
                name: "fk_usuario_role_role",
                table: "usuario_role");

            migrationBuilder.DropForeignKey(
                name: "fk_usuario_role_usuario",
                table: "usuario_role");

            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropPrimaryKey(
                name: "pk_usuario",
                table: "usuario");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role",
                table: "role");

            migrationBuilder.DropPrimaryKey(
                name: "pk_producto",
                table: "producto");

            migrationBuilder.DropPrimaryKey(
                name: "pk_marca",
                table: "marca");

            migrationBuilder.DropPrimaryKey(
                name: "pk_factura",
                table: "factura");

            migrationBuilder.DropIndex(
                name: "idx_factura_cliente_id",
                table: "factura");

            migrationBuilder.DropPrimaryKey(
                name: "pk_categoria",
                table: "categoria");

            migrationBuilder.DropPrimaryKey(
                name: "pk_usuario_role",
                table: "usuario_role");

            migrationBuilder.DropPrimaryKey(
                name: "pk_detalle_factura",
                table: "detalle_factura");

            migrationBuilder.DropColumn(
                name: "cai_dte",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "cliente_direccion",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "cliente_id",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "cliente_nit_dui",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "cliente_nombre",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "fecha_limite_emision",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "isr",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "iva",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "serie_factura",
                table: "factura");

            migrationBuilder.DropColumn(
                name: "tipo_documento",
                table: "factura");

            migrationBuilder.RenameTable(
                name: "usuario",
                newName: "Usuario");

            migrationBuilder.RenameTable(
                name: "role",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "producto",
                newName: "Producto");

            migrationBuilder.RenameTable(
                name: "marca",
                newName: "Marca");

            migrationBuilder.RenameTable(
                name: "factura",
                newName: "Factura");

            migrationBuilder.RenameTable(
                name: "categoria",
                newName: "Categoria");

            migrationBuilder.RenameTable(
                name: "usuario_role",
                newName: "UsuarioRole");

            migrationBuilder.RenameTable(
                name: "detalle_factura",
                newName: "DetalleFactura");

            migrationBuilder.RenameColumn(
                name: "salt",
                table: "Usuario",
                newName: "Salt");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Usuario",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "activo",
                table: "Usuario",
                newName: "Activo");

            migrationBuilder.RenameColumn(
                name: "ultimo_acceso",
                table: "Usuario",
                newName: "UltimoAcceso");

            migrationBuilder.RenameColumn(
                name: "nombre_usuario",
                table: "Usuario",
                newName: "NombreUsuario");

            migrationBuilder.RenameColumn(
                name: "contraseña_hash",
                table: "Usuario",
                newName: "ContraseñaHash");

            migrationBuilder.RenameColumn(
                name: "id_usuario",
                table: "Usuario",
                newName: "IDUsuario");

            migrationBuilder.RenameIndex(
                name: "uk_usuario_email",
                table: "Usuario",
                newName: "IX_Usuario_Email");

            migrationBuilder.RenameIndex(
                name: "idx_usuario_nombre_usuario",
                table: "Usuario",
                newName: "IX_Usuario_NombreUsuario");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Role",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "descripcion",
                table: "Role",
                newName: "Descripcion");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Role",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "uk_role_nombre",
                table: "Role",
                newName: "IX_Role_Nombre");

            migrationBuilder.RenameColumn(
                name: "stock",
                table: "Producto",
                newName: "Stock");

            migrationBuilder.RenameColumn(
                name: "activo",
                table: "Producto",
                newName: "Activo");

            migrationBuilder.RenameColumn(
                name: "stock_minimo",
                table: "Producto",
                newName: "StockMinimo");

            migrationBuilder.RenameColumn(
                name: "nombre_producto",
                table: "Producto",
                newName: "NombreProducto");

            migrationBuilder.RenameColumn(
                name: "id_marca",
                table: "Producto",
                newName: "IDMarca");

            migrationBuilder.RenameColumn(
                name: "id_categoria",
                table: "Producto",
                newName: "IDCategoria");

            migrationBuilder.RenameColumn(
                name: "id_producto",
                table: "Producto",
                newName: "IDProducto");

            migrationBuilder.RenameIndex(
                name: "uk_producto_codigo",
                table: "Producto",
                newName: "IX_Producto_codigo");

            migrationBuilder.RenameIndex(
                name: "IX_producto_id_marca",
                table: "Producto",
                newName: "IX_Producto_IDMarca");

            migrationBuilder.RenameIndex(
                name: "IX_producto_id_categoria",
                table: "Producto",
                newName: "IX_Producto_IDCategoria");

            migrationBuilder.RenameColumn(
                name: "nombre_marca",
                table: "Marca",
                newName: "NombreMarca");

            migrationBuilder.RenameColumn(
                name: "id_marca",
                table: "Marca",
                newName: "IDMarca");

            migrationBuilder.RenameColumn(
                name: "total",
                table: "Factura",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "estado",
                table: "Factura",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "usuario_id",
                table: "Factura",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "sub_total",
                table: "Factura",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "numero_factura",
                table: "Factura",
                newName: "NumeroFactura");

            migrationBuilder.RenameColumn(
                name: "fecha_vencimiento",
                table: "Factura",
                newName: "FechaVencimiento");

            migrationBuilder.RenameColumn(
                name: "fecha_creacion",
                table: "Factura",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "id_factura",
                table: "Factura",
                newName: "IDFactura");

            migrationBuilder.RenameColumn(
                name: "otros_impuestos",
                table: "Factura",
                newName: "Impuesto");

            migrationBuilder.RenameIndex(
                name: "uk_factura_numero_factura",
                table: "Factura",
                newName: "IX_Factura_NumeroFactura");

            migrationBuilder.RenameIndex(
                name: "idx_factura_usuario_id",
                table: "Factura",
                newName: "IX_Factura_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "idx_factura_fecha_creacion",
                table: "Factura",
                newName: "IX_Factura_FechaCreacion");

            migrationBuilder.RenameIndex(
                name: "idx_factura_estado",
                table: "Factura",
                newName: "IX_Factura_Estado");

            migrationBuilder.RenameColumn(
                name: "nombre_categoria",
                table: "Categoria",
                newName: "NombreCategoria");

            migrationBuilder.RenameColumn(
                name: "id_categoria",
                table: "Categoria",
                newName: "IDCategoria");

            migrationBuilder.RenameColumn(
                name: "asignado_en",
                table: "UsuarioRole",
                newName: "AsignadoEn");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "UsuarioRole",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "usuario_id",
                table: "UsuarioRole",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_usuario_role_role_id",
                table: "UsuarioRole",
                newName: "IX_UsuarioRole_RoleId");

            migrationBuilder.RenameColumn(
                name: "descuento",
                table: "DetalleFactura",
                newName: "Descuento");

            migrationBuilder.RenameColumn(
                name: "cantidad",
                table: "DetalleFactura",
                newName: "Cantidad");

            migrationBuilder.RenameColumn(
                name: "precio_unitario",
                table: "DetalleFactura",
                newName: "PrecioUnitario");

            migrationBuilder.RenameColumn(
                name: "id_producto",
                table: "DetalleFactura",
                newName: "IDProducto");

            migrationBuilder.RenameColumn(
                name: "id_factura",
                table: "DetalleFactura",
                newName: "IDFactura");

            migrationBuilder.RenameColumn(
                name: "id_detalle_factura",
                table: "DetalleFactura",
                newName: "IDDetalleFactura");

            migrationBuilder.RenameIndex(
                name: "idx_detalle_factura_id_producto",
                table: "DetalleFactura",
                newName: "IX_DetalleFactura_IDProducto");

            migrationBuilder.RenameIndex(
                name: "idx_detalle_factura_id_factura",
                table: "DetalleFactura",
                newName: "IX_DetalleFactura_IDFactura");

            migrationBuilder.AlterColumn<int>(
                name: "IDFactura",
                table: "DetalleFactura",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Usuario__60FD6F4FAA1CC3B0",
                table: "Usuario",
                column: "IDUsuario");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Producto__ABDAF2B48FF32BD9",
                table: "Producto",
                column: "IDProducto");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Marca__CEC375E729BA7C81",
                table: "Marca",
                column: "IDMarca");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Factura__492FE9390232D7A0",
                table: "Factura",
                column: "IDFactura");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Categori__70E82E28335B1CB4",
                table: "Categoria",
                column: "IDCategoria");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioRole",
                table: "UsuarioRole",
                columns: new[] { "UsuarioId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__DetalleF__EF0E5D9A0E87D534",
                table: "DetalleFactura",
                column: "IDDetalleFactura");

            migrationBuilder.AddForeignKey(
                name: "FK__DetalleFa__IDFac__33D4B598",
                table: "DetalleFactura",
                column: "IDFactura",
                principalTable: "Factura",
                principalColumn: "IDFactura");

            migrationBuilder.AddForeignKey(
                name: "FK__DetalleFa__IDPro__34C8D9D1",
                table: "DetalleFactura",
                column: "IDProducto",
                principalTable: "Producto",
                principalColumn: "IDProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Factura_Usuario",
                table: "Factura",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "IDUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Producto__IDCate__2F10007B",
                table: "Producto",
                column: "IDCategoria",
                principalTable: "Categoria",
                principalColumn: "IDCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK__Producto__IDMarc__2E1BDC42",
                table: "Producto",
                column: "IDMarca",
                principalTable: "Marca",
                principalColumn: "IDMarca");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRole_Role",
                table: "UsuarioRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRole_Usuario",
                table: "UsuarioRole",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "IDUsuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
