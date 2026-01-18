using System.Collections.Generic;
using System.Linq;
using Fatura.Models.Facturacion;
using Fatura.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Fatura.Services
{
    public class FacturaTicketService : IFacturaTicketService
    {
        public byte[] GenerarTicket(Factura factura)
        {
            var detalles = factura.DetalleFacturas?.ToList()
                           ?? new List<DetalleFactura>();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // 80mm ≈ 226 puntos
                    page.Size(new PageSize(226, 800));

                    page.Margin(5);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Content().Column(col =>
                    {
                        col.Spacing(5);

                        col.Item()
                           .AlignCenter()
                           .Text("MI TIENDA")
                           .Bold()
                           .FontSize(12);

                        col.Item()
                           .AlignCenter()
                           .Text($"Factura #{factura.NumeroFactura}");

                        col.Item().LineHorizontal(1);

                        col.Item().Text($"Cliente: {factura.ClienteNombre}");
                        col.Item().Text($"Fecha: {factura.FechaCreacion:dd/MM/yyyy HH:mm}");

                        col.Item().LineHorizontal(1);

                        foreach (var item in detalles)
                        {
                            col.Item().Text(item.NombreProducto);

                            col.Item().Row(row =>
                            {
                                row.RelativeItem()
                                   .Text($"{item.Cantidad} x {item.PrecioUnitario:C}");

                                row.ConstantItem(50)
                                   .AlignRight()
                                   .Text(item.Total.ToString("C"));
                            });
                        }

                        col.Item().LineHorizontal(1);

                        col.Item()
                           .AlignRight()
                           .Text($"TOTAL: {factura.Total:C}")
                           .Bold()
                           .FontSize(11);

                        col.Item()
                           .PaddingTop(10)
                           .AlignCenter()
                           .Text("Gracias por su compra");
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
