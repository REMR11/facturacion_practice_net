using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fatura.Migrations
{
    /// <inheritdoc />
    public partial class DetalleFacturaCantidadDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "cantidad",
                table: "detalle_factura",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "cantidad",
                table: "detalle_factura",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");
        }
    }
}
