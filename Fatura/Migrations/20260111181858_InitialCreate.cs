using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fatura.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    IDCategoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreCategoria = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__70E82E28335B1CB4", x => x.IDCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Factura",
                columns: table => new
                {
                    IDFactura = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Total = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Factura__492FE9390232D7A0", x => x.IDFactura);
                });

            migrationBuilder.CreateTable(
                name: "Marca",
                columns: table => new
                {
                    IDMarca = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreMarca = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Marca__CEC375E729BA7C81", x => x.IDMarca);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IDUsurio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreUSuario = table.Column<string>(type: "character varying(40)", unicode: false, maxLength: 40, nullable: true),
                    Contraseña = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__60FD6F4FAA1CC3B0", x => x.IDUsurio);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    IDProducto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IDMarca = table.Column<int>(type: "integer", nullable: true),
                    IDCategoria = table.Column<int>(type: "integer", nullable: true),
                    NombreProducto = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    precio = table.Column<double>(type: "double precision", nullable: true),
                    codigo = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__ABDAF2B48FF32BD9", x => x.IDProducto);
                    table.ForeignKey(
                        name: "FK__Producto__IDCate__2F10007B",
                        column: x => x.IDCategoria,
                        principalTable: "Categoria",
                        principalColumn: "IDCategoria");
                    table.ForeignKey(
                        name: "FK__Producto__IDMarc__2E1BDC42",
                        column: x => x.IDMarca,
                        principalTable: "Marca",
                        principalColumn: "IDMarca");
                });

            migrationBuilder.CreateTable(
                name: "DetalleFactura",
                columns: table => new
                {
                    IDDetalleFactura = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IDFactura = table.Column<int>(type: "integer", nullable: true),
                    IDProducto = table.Column<int>(type: "integer", nullable: true),
                    FechaCompra = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Total = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DetalleF__EF0E5D9A0E87D534", x => x.IDDetalleFactura);
                    table.ForeignKey(
                        name: "FK__DetalleFa__IDFac__33D4B598",
                        column: x => x.IDFactura,
                        principalTable: "Factura",
                        principalColumn: "IDFactura");
                    table.ForeignKey(
                        name: "FK__DetalleFa__IDPro__34C8D9D1",
                        column: x => x.IDProducto,
                        principalTable: "Producto",
                        principalColumn: "IDProducto");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFactura_IDFactura",
                table: "DetalleFactura",
                column: "IDFactura");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFactura_IDProducto",
                table: "DetalleFactura",
                column: "IDProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IDCategoria",
                table: "Producto",
                column: "IDCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IDMarca",
                table: "Producto",
                column: "IDMarca");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleFactura");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Factura");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Marca");
        }
    }
}
