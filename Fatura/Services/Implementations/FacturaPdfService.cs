using Fatura.Models.Facturacion;
using Fatura.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Fatura.Services.Implementations
{
    public class FacturaPdfService : IFacturaPdfService
    {
        public byte[] GenerarPdf(Factura factura)
        {
            var detalles = factura.DetalleFacturas?.ToList() ?? new List<DetalleFactura>();
            var moneda = string.IsNullOrWhiteSpace(factura.MonedaSimbolo) ? "S/" : factura.MonedaSimbolo;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Text("Factura").FontSize(20).Bold();
                        row.ConstantItem(200).AlignRight().Text(text =>
                        {
                            text.Span("Número: ").SemiBold();
                            text.Span(factura.NumeroFactura ?? "—");
                        });
                    });

                    page.Content().Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Cliente").Bold();
                                col.Item().Text(factura.ClienteNombre ?? "—");
                                col.Item().Text(factura.ClienteNitDui ?? "—");
                                col.Item().Text(factura.ClienteDireccion ?? "—");
                            });

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Documento").Bold();
                                col.Item().Text($"Tipo: {factura.TipoDocumento}");
                                col.Item().Text($"Serie: {factura.SerieFactura ?? "—"}");
                                col.Item().Text($"Emisión: {factura.FechaCreacion:yyyy-MM-dd}");
                                col.Item().Text($"Venc.: {factura.FechaVencimiento:yyyy-MM-dd}");
                            });
                        });

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(4);
                                columns.ConstantColumn(60);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(90);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellHeader).Text("Producto/Servicio");
                                header.Cell().Element(CellHeader).AlignCenter().Text("Cant.");
                                header.Cell().Element(CellHeader).AlignRight().Text("Precio");
                                header.Cell().Element(CellHeader).AlignRight().Text("Desc.");
                                header.Cell().Element(CellHeader).AlignRight().Text("Subtotal");
                            });

                            foreach (var detalle in detalles)
                            {
                                table.Cell().Element(CellBody).Text(detalle.NombreProducto);
                                table.Cell().Element(CellBody).AlignCenter().Text(detalle.Cantidad.ToString());
                                table.Cell().Element(CellBody).AlignRight().Text($"{moneda}{detalle.PrecioUnitario:F2}");
                                table.Cell().Element(CellBody).AlignRight().Text($"{moneda}{detalle.Descuento:F2}");
                                table.Cell().Element(CellBody).AlignRight().Text($"{moneda}{detalle.Total:F2}");
                            }

                            if (detalles.Count == 0)
                            {
                                table.Cell().ColumnSpan(5).Element(CellBody).AlignCenter().Text("Sin ítems");
                            }
                        });

                        column.Item().AlignRight().Column(col =>
                        {
                            col.Item().Text($"Subtotal: {moneda}{factura.SubTotal:F2}");
                            col.Item().Text($"IGV (18%): {moneda}{factura.Iva:F2}");
                            col.Item().Text($"Total: {moneda}{factura.Total:F2}").Bold();
                        });
                    });

                    page.Footer().AlignCenter().Text("Documento generado por el sistema").FontSize(9).FontColor(Colors.Grey.Darken2);
                });
            });

            return document.GeneratePdf();
        }

        private static IContainer CellHeader(IContainer container)
        {
            return container.DefaultTextStyle(x => x.SemiBold())
                .PaddingVertical(6)
                .PaddingHorizontal(4)
                .Background(Colors.Grey.Lighten3);
        }

        private static IContainer CellBody(IContainer container)
        {
            return container.PaddingVertical(4).PaddingHorizontal(4);
        }
    }
}
