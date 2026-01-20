using System;
using System.Linq;
using System.Text.Json;
using Fatura.Models;
using Fatura.Models.Facturacion;
using Fatura.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Fatura.Services.Implementations
{
    public class FacturaPdfService : IFacturaPdfService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IQrService _qrService;
        private readonly xstoreContext _context;

        public FacturaPdfService(IWebHostEnvironment webHostEnvironment, IQrService qrService, xstoreContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _qrService = qrService;
            _context = context;
        }

        private bool EsCreditoFiscal(Factura factura)
        {
            return factura.TipoDocumento == "03" || 
                   factura.TipoDocumento.Contains("Crédito Fiscal", StringComparison.OrdinalIgnoreCase) ||
                   factura.TipoDocumento.Contains("Credito Fiscal", StringComparison.OrdinalIgnoreCase);
        }

        private string ObtenerTituloDocumento(Factura factura)
        {
            return EsCreditoFiscal(factura) ? "Comprobante de Crédito Fiscal" : "Factura";
        }
        private string ObtenerSimboloMoneda(string monedaSimbolo)
        {
            if (string.IsNullOrWhiteSpace(monedaSimbolo))
                return "S/";
            
            // Si viene "USD" convertir a "$"
            if (monedaSimbolo.Equals("USD", StringComparison.OrdinalIgnoreCase))
                return "$";
            
            return monedaSimbolo;
        }

        private (string Nit, string Direccion) ObtenerDatosEmpresa()
        {
            try
            {
                var empresa = _context.Set<Models.Catalogos.Empresa>().FirstOrDefault();
                if (empresa != null)
                {
                    return (empresa.Nit ?? "02614612-5", empresa.Direccion ?? "3 CALLE PONIENTE, #3-7B, MUNICIPIO DE SANTA TECLA, DEPARTAMENTO DE LA LIBERTAD");
                }
            }
            catch
            {
                // Si falla, usar valores por defecto
            }
            return ("02614612-5", "3 CALLE PONIENTE, #3-7B, MUNICIPIO DE SANTA TECLA, DEPARTAMENTO DE LA LIBERTAD");
        }

        private string? ObtenerRutaLogo()
        {
            try
            {
                // Intentar cargar el logo más reciente primero
                var logoPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "pr rodriguez v3_Mesa de trabajo 1.png");
                if (System.IO.File.Exists(logoPath))
                {
                    return logoPath;
                }

                // Intentar cargar LOGO.png
                logoPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "LOGO.png");
                if (System.IO.File.Exists(logoPath))
                {
                    return logoPath;
                }

                // Intentar cargar desde wwwroot/images/logo.png
                logoPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "images", "logo.png");
                if (System.IO.File.Exists(logoPath))
                {
                    return logoPath;
                }

                // Intentar cargar desde wwwroot/logo.png
                logoPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "logo.png");
                if (System.IO.File.Exists(logoPath))
                {
                    return logoPath;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public byte[] GenerarPdf(Factura factura)
        {
            try
            {
                if (factura == null)
                {
                    throw new ArgumentNullException(nameof(factura), "La factura no puede ser nula.");
                }

                var detalles = factura.DetalleFacturas?.ToList() ?? new List<DetalleFactura>();
                var moneda = ObtenerSimboloMoneda(factura.MonedaSimbolo ?? "S/");

                var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Column(col =>
                    {
                        // Logo y título
                        col.Item().Row(row =>
                        {
                            var logoPath = ObtenerRutaLogo();
                            if (!string.IsNullOrEmpty(logoPath) && System.IO.File.Exists(logoPath))
                            {
                                try
                                {
                                    row.ConstantItem(80).Height(60).Image(logoPath).FitArea();
                                }
                                catch
                                {
                                    // Si falla, continuar sin logo
                                }
                            }
                            row.RelativeItem().Column(headerCol =>
                            {
                                headerCol.Item().Text(ObtenerTituloDocumento(factura)).FontSize(20).Bold();
                                headerCol.Item().Text(text =>
                                {
                                    text.Span("Número: ").SemiBold();
                                    text.Span(factura.NumeroFactura ?? "—");
                                });
                                
                                // Agregar NIT y dirección de la empresa
                                var (nit, direccion) = ObtenerDatosEmpresa();
                                headerCol.Item().PaddingTop(5).Text(text =>
                                {
                                    text.Span("NIT: ").SemiBold();
                                    text.Span(nit);
                                });
                                headerCol.Item().Text(text =>
                                {
                                    text.Span("Dirección: ").SemiBold();
                                    text.Span(direccion);
                                });
                            });
                        });
                        col.Item().PaddingBottom(10);
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
                                col.Item().Text($"Emisión: {factura.FechaCreacion:dd/MM/yyyy}");
                                if (factura.FechaVencimiento.HasValue)
                                {
                                    col.Item().Text($"Vencimiento: {factura.FechaVencimiento.Value:dd/MM/yyyy}");
                                }
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
                            col.Item().Text($"IGV (13%): {moneda}{factura.Iva:F2}");
                            col.Item().Text($"Total: {moneda}{factura.Total:F2}").Bold();
                        });

                        // Agregar QR si es crédito fiscal
                        if (EsCreditoFiscal(factura))
                        {
                            column.Item().PaddingTop(20).Column(qrCol =>
                            {
                                qrCol.Item().AlignCenter().Text("Código QR Crédito Fiscal").Bold().FontSize(10);
                                try
                                {
                                    var qrData = GenerarDatosQr(factura);
                                    var qrBytes = _qrService.GenerarQr(qrData, 150);
                                    qrCol.Item()
                                       .PaddingTop(5)
                                       .AlignCenter()
                                       .Height(150)
                                       .Image(qrBytes)
                                       .FitArea();
                                }
                                catch
                                {
                                    qrCol.Item().AlignCenter().Text("Error al generar QR").FontSize(8).FontColor(Colors.Red.Medium);
                                }
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text("Documento generado por el sistema").FontSize(9).FontColor(Colors.Grey.Darken2);
                });
            });

                return document.GeneratePdf();
            }
            catch (Exception ex)
            {
                // Log del error (puedes usar un logger aquí)
                System.Diagnostics.Debug.WriteLine($"Error al generar PDF: {ex.Message}");
                throw new Exception($"Error al generar el PDF de la factura: {ex.Message}", ex);
            }
        }

        private string GenerarDatosQr(Factura factura)
        {
            var datos = new
            {
                tipo = "CreditoFiscal",
                numero = factura.NumeroFactura,
                serie = factura.SerieFactura ?? "",
                nit = factura.ClienteNitDui,
                nombre = factura.ClienteNombre,
                fecha = factura.FechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss"),
                total = factura.Total.ToString("F2"),
                moneda = factura.MonedaSimbolo ?? "USD"
            };

            return JsonSerializer.Serialize(datos);
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
