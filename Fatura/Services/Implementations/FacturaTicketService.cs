using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using Fatura.Models.Facturacion;
using Fatura.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Fatura.Services
{
    [SupportedOSPlatform("windows")]
    public class FacturaTicketService : IFacturaTicketService
    {
        private List<string> lineasImprimir = new List<string>();
        private int anchoTicket = 32; // Caracteres por línea para impresora de 80mm

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

        public bool ImprimirTicket(Factura factura, string nombreImpresora = "RPT004")
        {
            try
            {
                lineasImprimir = new List<string>();
                var detalles = factura.DetalleFacturas?.ToList() ?? new List<DetalleFactura>();
                var moneda = string.IsNullOrWhiteSpace(factura.MonedaSimbolo) ? "S/" : factura.MonedaSimbolo;

                // Construir el contenido del ticket
                AgregarLineaCentrada("TALLER RODRIGUEZ");
                AgregarLineaCentrada("SERVICIO AUTOMOTRIZ");
                AgregarLineaCentrada("Dirrecion: 123456789");
                AgregarLineaCentrada("Tel: 999-999-999");
                AgregarLinea("");
                AgregarLineaCentrada("================================");
                AgregarLineaCentrada($"Factura #{factura.NumeroFactura}");
                AgregarLinea("");
                AgregarLinea($"Cliente: {factura.ClienteNombre}");
                AgregarLinea($"NIT/DUI: {factura.ClienteNitDui}");
                AgregarLinea($"Fecha: {factura.FechaCreacion:dd/MM/yyyy HH:mm}");
                AgregarLinea("================================");
                AgregarLinea("");

                // Detalles de productos
                foreach (var item in detalles)
                {
                    AgregarLinea(TruncarTexto(item.NombreProducto, anchoTicket));
                    var lineaDetalle = $"{item.Cantidad} x {moneda}{item.PrecioUnitario:F2}";
                    var total = $"{moneda}{item.Total:F2}";
                    AgregarLineaConTotal(lineaDetalle, total);
                }

                AgregarLinea("");
                AgregarLinea("================================");
                
                // Totales
                AgregarLineaConTotal("Subtotal:", $"{moneda}{factura.SubTotal:F2}");
                AgregarLineaConTotal("IGV (18%):", $"{moneda}{factura.Iva:F2}");
                AgregarLinea("================================");
                AgregarLineaConTotal("TOTAL:", $"{moneda}{factura.Total:F2}");
                AgregarLinea("================================");
                AgregarLinea("");
                AgregarLineaCentrada("!Gracias por su compra ¡");
                AgregarLineaCentrada("Vuelva Pronto");
                AgregarLinea("");
                AgregarLinea("");
                AgregarLinea("");

                // Imprimir usando PrintDocument
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = nombreImpresora;

                // Verificar si la impresora existe
                if (!pd.PrinterSettings.IsValid)
                {
                    // Intentar buscar la impresora por nombre parcial
                    bool encontrada = false;
                    foreach (string printer in PrinterSettings.InstalledPrinters)
                    {
                        if (printer.Contains("RPT") || printer.Contains("POS") || printer.Contains("Thermal"))
                        {
                            pd.PrinterSettings.PrinterName = printer;
                            encontrada = true;
                            break;
                        }
                    }

                    if (!encontrada)
                    {
                        throw new Exception($"No se encontró la impresora '{nombreImpresora}'. Impresoras disponibles: {string.Join(", ", PrinterSettings.InstalledPrinters.Cast<string>())}");
                    }
                }

                pd.PrintPage += new PrintPageEventHandler(ImprimirPagina);
                pd.Print();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al imprimir ticket: {ex.Message}", ex);
            }
        }

        private void ImprimirPagina(object sender, PrintPageEventArgs e)
        {
            if (e.Graphics == null)
                return;

            Graphics graphics = e.Graphics;
            Font fuente = new Font("Courier New", 9);
            Font fuenteNegrita = new Font("Courier New", 9, FontStyle.Bold);
            float yPos = 0;
            int count = 0;
            float leftMargin = 0;
            float topMargin = 0;

            foreach (string linea in lineasImprimir)
            {
                yPos = topMargin + (count * fuente.GetHeight(graphics));
                
                // Usar negrita para líneas con "TOTAL" o encabezados
                Font fuenteActual = (linea.Contains("TOTAL") || linea.Contains("MI TIENDA")) 
                    ? fuenteNegrita 
                    : fuente;

                graphics.DrawString(linea, fuenteActual, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }

            e.HasMorePages = false;
        }

        private void AgregarLinea(string texto)
        {
            lineasImprimir.Add(texto);
        }

        private void AgregarLineaCentrada(string texto)
        {
            int espacios = (anchoTicket - texto.Length) / 2;
            if (espacios > 0)
            {
                lineasImprimir.Add(new string(' ', espacios) + texto);
            }
            else
            {
                lineasImprimir.Add(texto);
            }
        }

        private void AgregarLineaConTotal(string izquierda, string derecha)
        {
            int espacios = anchoTicket - izquierda.Length - derecha.Length;
            if (espacios > 0)
            {
                lineasImprimir.Add(izquierda + new string(' ', espacios) + derecha);
            }
            else
            {
                lineasImprimir.Add(izquierda + " " + derecha);
            }
        }

        private string TruncarTexto(string texto, int maxLength)
        {
            if (string.IsNullOrEmpty(texto))
                return string.Empty;

            return texto.Length <= maxLength ? texto : texto.Substring(0, maxLength - 3) + "...";
        }
    }
}
