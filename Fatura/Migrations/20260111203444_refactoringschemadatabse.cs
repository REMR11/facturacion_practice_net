using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fatura.Migrations
{
    /// <inheritdoc />
    public partial class refactoringschemadatabse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaCompra",
                table: "DetalleFactura");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "DetalleFactura");

            migrationBuilder.RenameColumn(
                name: "NombreUSuario",
                table: "Usuario",
                newName: "NombreUsuario");

            migrationBuilder.RenameColumn(
                name: "Contraseña",
                table: "Usuario",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "IDUsurio",
                table: "Usuario",
                newName: "IDUsuario");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Usuario",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "ContraseñaHash",
                table: "Usuario",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Usuario",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Usuario",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Usuario",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Usuario",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Usuario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Usuario",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Usuario",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimoAcceso",
                table: "Usuario",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Usuario",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "precio",
                table: "Producto",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Producto",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Producto",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Producto",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Producto",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Producto",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Producto",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Producto",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StockMinimo",
                table: "Producto",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Producto",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Producto",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Marca",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Marca",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Marca",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Marca",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Marca",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Marca",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Marca",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Factura",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "Factura",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Factura",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Factura",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Factura",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Factura",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Factura",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaVencimiento",
                table: "Factura",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Impuesto",
                table: "Factura",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Factura",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NumeroFactura",
                table: "Factura",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "Factura",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Factura",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Factura",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Factura",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "DetalleFactura",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DetalleFactura",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "DetalleFactura",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "DetalleFactura",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "DetalleFactura",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Descuento",
                table: "DetalleFactura",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DetalleFactura",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioUnitario",
                table: "DetalleFactura",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DetalleFactura",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "DetalleFactura",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Categoria",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Categoria",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Categoria",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Categoria",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categoria",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Categoria",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Categoria",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRole",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    AsignadoEn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRole", x => new { x.UsuarioId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UsuarioRole_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRole_Usuario",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "IDUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_NombreUsuario",
                table: "Usuario",
                column: "NombreUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_codigo",
                table: "Producto",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factura_Estado",
                table: "Factura",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_FechaCreacion",
                table: "Factura",
                column: "FechaCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Factura_NumeroFactura",
                table: "Factura",
                column: "NumeroFactura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factura_UsuarioId",
                table: "Factura",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Nombre",
                table: "Role",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRole_RoleId",
                table: "UsuarioRole",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Factura_Usuario",
                table: "Factura",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "IDUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factura_Usuario",
                table: "Factura");

            migrationBuilder.DropTable(
                name: "UsuarioRole");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_Email",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_NombreUsuario",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Producto_codigo",
                table: "Producto");

            migrationBuilder.DropIndex(
                name: "IX_Factura_Estado",
                table: "Factura");

            migrationBuilder.DropIndex(
                name: "IX_Factura_FechaCreacion",
                table: "Factura");

            migrationBuilder.DropIndex(
                name: "IX_Factura_NumeroFactura",
                table: "Factura");

            migrationBuilder.DropIndex(
                name: "IX_Factura_UsuarioId",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "ContraseñaHash",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "UltimoAcceso",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "StockMinimo",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Marca");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Marca");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Marca");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Marca");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Marca");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Marca");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Marca");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "FechaVencimiento",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "Impuesto",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "NumeroFactura",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Factura");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "DetalleFactura");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DetalleFactura");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DetalleFactura");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "DetalleFactura");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "DetalleFactura");

            migrationBuilder.DropColumn(
                name: "Descuento",
                table: "DetalleFactura");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DetalleFactura");

            migrationBuilder.DropColumn(
                name: "PrecioUnitario",
                table: "DetalleFactura");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DetalleFactura");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DetalleFactura");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Categoria");

            migrationBuilder.RenameColumn(
                name: "NombreUsuario",
                table: "Usuario",
                newName: "NombreUSuario");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Usuario",
                newName: "Contraseña");

            migrationBuilder.RenameColumn(
                name: "IDUsuario",
                table: "Usuario",
                newName: "IDUsurio");

            migrationBuilder.AlterColumn<double>(
                name: "precio",
                table: "Producto",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Factura",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "Factura",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCompra",
                table: "DetalleFactura",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "DetalleFactura",
                type: "money",
                nullable: true);
        }
    }
}
