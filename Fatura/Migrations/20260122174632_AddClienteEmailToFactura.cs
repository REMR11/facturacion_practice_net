using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fatura.Migrations
{
    /// <inheritdoc />
    public partial class AddClienteEmailToFactura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cliente_email",
                table: "factura",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cliente_email",
                table: "factura");
        }
    }
}
