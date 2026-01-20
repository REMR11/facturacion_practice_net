using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using Fatura.Models;
using Fatura.Models.Facturacion;
using Fatura.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Fatura.Services
{
    [SupportedOSPlatform("windows")]
    public class FacturaTicketService : IFacturaTicketService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IQrService _qrService;
        private readonly xstoreContext _context;
        private List<string> lineasImprimir = new List<string>();
        private int anchoTicket = 32; // Caracteres por línea para impresora de 80mm

        public FacturaTicketService(IWebHostEnvironment webHostEnvironment, IQrService qrService, xstoreContext context)
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
            return EsCreditoFiscal(factura) ? "COMPROBANTE DE CRÉDITO FISCAL" : "FACTURA";
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

        private string ObtenerNitEmpresa()
        {
            try
            {
                var empresa = _context.Set<Models.Catalogos.Empresa>().FirstOrDefault();
                if (empresa != null && !string.IsNullOrEmpty(empresa.Nit))
                {
                    return empresa.Nit;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener NIT de empresa: {ex.Message}");
            }
            // Valor por defecto si no se puede obtener de la BD
            return "02614612-5";
        }

        private System.Drawing.Image? ObtenerLogo()
        {
            try
            {
                // Cargar SOLO LOGO.png (logo principal)
                var logoPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "LOGO.png");
                if (!System.IO.File.Exists(logoPath))
                {
                    System.Diagnostics.Debug.WriteLine($"LOGO.png no encontrado en: {logoPath}");
                    return null;
                }

                System.Diagnostics.Debug.WriteLine($"Cargando LOGO.png desde: {logoPath}");
                var logoOriginal = System.Drawing.Image.FromFile(logoPath);
                System.Diagnostics.Debug.WriteLine($"Logo original cargado: {logoOriginal.Width}x{logoOriginal.Height} píxeles");

                // Convertir a Bitmap con fondo negro para mejor visibilidad
                var bitmap = new System.Drawing.Bitmap(logoOriginal.Width, logoOriginal.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (var g = System.Drawing.Graphics.FromImage(bitmap))
                {
                    // Rellenar fondo negro primero
                    g.Clear(System.Drawing.Color.Black);
                    // Dibujar el logo sobre fondo negro
                    g.DrawImage(logoOriginal, 0, 0, logoOriginal.Width, logoOriginal.Height);
                }
                logoOriginal.Dispose();

                System.Diagnostics.Debug.WriteLine($"Logo procesado: {bitmap.Width}x{bitmap.Height} píxeles");
                return bitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR al cargar LOGO.png: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public byte[] GenerarTicket(Factura factura)
        {
            var detalles = factura.DetalleFacturas?.ToList()
                           ?? new List<DetalleFactura>();
            
            var moneda = ObtenerSimboloMoneda(factura.MonedaSimbolo);

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

                        // Logo LOGO.png si existe (más grande)
                        var logoPath = ObtenerRutaLogo();
                        if (!string.IsNullOrEmpty(logoPath) && System.IO.File.Exists(logoPath))
                        {
                            try
                            {
                                col.Item()
                                   .AlignCenter()
                                   .Height(100) // Aumentado de 50 a 100 para mejor visibilidad
                                   .Image(logoPath)
                                   .FitArea();
                            }
                            catch
                            {
                                // Si falla cargar imagen, mostrar solo texto
                                col.Item()
                                   .AlignCenter()
                                   .Text("TALLER RODRIGUEZ")
                                   .Bold()
                                   .FontSize(12);
                            }
                        }
                        else
                        {
                            col.Item()
                               .AlignCenter()
                               .Text("TALLER RODRIGUEZ")
                               .Bold()
                               .FontSize(12);
                        }

                        col.Item()
                           .AlignCenter()
                           .Text($"{ObtenerTituloDocumento(factura)} #{factura.NumeroFactura}");

                        col.Item().LineHorizontal(1);

                        // Agregar NIT de la empresa
                        var nitEmpresa = ObtenerNitEmpresa();
                        if (!string.IsNullOrEmpty(nitEmpresa))
                        {
                            col.Item()
                               .AlignCenter()
                               .Text($"NIT: {nitEmpresa}")
                               .FontSize(8);
                        }

                        col.Item().Text($"Cliente: {factura.ClienteNombre}");
                        col.Item().Text($"Fecha Emisión: {factura.FechaCreacion:dd/MM/yyyy}");
                        if (factura.FechaVencimiento.HasValue)
                        {
                            col.Item().Text($"Fecha Vencimiento: {factura.FechaVencimiento.Value:dd/MM/yyyy}");
                        }

                        col.Item().LineHorizontal(1);

                        foreach (var item in detalles)
                        {
                            col.Item().Text(item.NombreProducto);

                            col.Item().Row(row =>
                            {
                                row.RelativeItem()
                                   .Text($"{item.Cantidad} x {moneda}{item.PrecioUnitario:F2}");

                                row.ConstantItem(50)
                                   .AlignRight()
                                   .Text($"{moneda}{item.Total:F2}");
                            });
                        }

                        col.Item().LineHorizontal(1);

                        col.Item()
                           .AlignRight()
                           .Text($"TOTAL: {moneda}{factura.Total:F2}")
                           .Bold()
                           .FontSize(11);

                        col.Item()
                           .PaddingTop(10)
                           .AlignCenter()
                           .Text("Gracias por su compra");

                        // Agregar QR si es crédito fiscal
                        if (EsCreditoFiscal(factura))
                        {
                            try
                            {
                                var qrData = GenerarDatosQr(factura);
                                var qrBytes = _qrService.GenerarQr(qrData, 100);
                                
                                col.Item()
                                   .PaddingTop(10)
                                   .AlignCenter()
                                   .Height(100)
                                   .Image(qrBytes)
                                   .FitArea();
                                
                                col.Item()
                                   .AlignCenter()
                                   .Text("Código QR Crédito Fiscal")
                                   .FontSize(8);
                            }
                            catch
                            {
                                // Si falla, continuar sin QR
                            }
                        }
                    });
                });
            });

            return document.GeneratePdf();
        }

        public bool ImprimirTicket(Factura factura, string nombreImpresora = "RPT004")
        {
            try
            {
                if (factura == null)
                {
                    System.Diagnostics.Debug.WriteLine("Error: La factura es nula.");
                    return false;
                }

                EstablecerFacturaActual(factura);
                lineasImprimir = new List<string>();
                var detalles = factura.DetalleFacturas?.ToList() ?? new List<DetalleFactura>();
                var moneda = ObtenerSimboloMoneda(factura.MonedaSimbolo ?? "S/");

                // Construir el contenido del ticket
                // NOTA: El logo LOGO.png se dibuja PRIMERO en ImprimirPagina, antes de estas líneas de texto
                // El logo se dibuja antes, estas líneas van después del logo
                AgregarLinea("");
                AgregarLineaCentrada("TALLER RODRIGUEZ");
                AgregarLineaCentrada("SERVICIO AUTOMOTRIZ");
                
                // Agregar NIT de la empresa (siempre mostrar)
                var nitEmpresa = ObtenerNitEmpresa();
                AgregarLineaCentrada($"NIT: {nitEmpresa}");
                
                // Agregar dirección de la empresa (siempre mostrar)
                AgregarLineaCentrada("Dirección: 3 CALLE PONIENTE, #3-7B");
                AgregarLineaCentrada("MUNICIPIO DE SANTA TECLA");
                AgregarLineaCentrada("DEPARTAMENTO DE LA LIBERTAD");
                
                AgregarLineaCentrada("Correo: Jr9000390@gmail.com");
                AgregarLineaCentrada("Tel: +503 7595-7484");

                AgregarLinea("");
                AgregarLineaCentrada("================================");
                AgregarLineaCentrada($"{ObtenerTituloDocumento(factura)} #{factura.NumeroFactura}");
                AgregarLinea("");
                AgregarLinea($"Cliente: {factura.ClienteNombre}");
                AgregarLinea($"NIT/DUI: {factura.ClienteNitDui}");
                AgregarLinea($"Fecha Emisión: {factura.FechaCreacion:dd/MM/yyyy}");
                if (factura.FechaVencimiento.HasValue)
                {
                    AgregarLinea($"Fecha Vencimiento: {factura.FechaVencimiento.Value:dd/MM/yyyy}");
                }
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
                AgregarLineaConTotal("IGV (13%):", $"{moneda}{factura.Iva:F2}");
                AgregarLinea("================================");
                AgregarLineaConTotal("TOTAL:", $"{moneda}{factura.Total:F2}");
                AgregarLinea("================================");
                AgregarLinea("");
                AgregarLineaCentrada("!Gracias por su compra ¡");
                AgregarLineaCentrada("Vuelva Pronto");
                
                // Agregar QR si es crédito fiscal
                if (EsCreditoFiscal(factura))
                {
                    AgregarLinea("");
                    AgregarLineaCentrada("================================");
                    AgregarLineaCentrada("CÓDIGO QR CRÉDITO FISCAL");
                    AgregarLinea("");
                }
                else
                {
                    AgregarLinea("");
                    AgregarLinea("");
                    AgregarLinea("");
                }

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
                System.Diagnostics.Debug.WriteLine($"Error al imprimir ticket: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                // Retornar false en lugar de lanzar excepción para mejor manejo en el controlador
                return false;
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
            float topMargin = 5; // Margen superior inicial

            // Dibujar logo PRIMERO, antes de cualquier texto - SOLO LOGO.png
            var logo = ObtenerLogo();
            if (logo != null)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"Logo cargado: {logo.Width}x{logo.Height} píxeles");
                    System.Diagnostics.Debug.WriteLine($"Ancho de página: {e.PageBounds.Width} píxeles");
                    
                    // Hacer el logo MUY GRANDE para máxima visibilidad (150px altura)
                    int maxHeight = 150; // Aumentado significativamente
                    float ratio = (float)logo.Height / logo.Width;
                    int newWidth = (int)(maxHeight / ratio);
                    int newHeight = maxHeight;
                    
                    // Asegurar que no sea muy ancho (máx 280px para 80mm de papel)
                    int maxWidth = 280; // Aumentado para logo más grande
                    if (newWidth > maxWidth)
                    {
                        newWidth = maxWidth;
                        newHeight = (int)(newWidth * ratio);
                    }

                    System.Diagnostics.Debug.WriteLine($"Logo redimensionado a: {newWidth}x{newHeight} píxeles");

                    // Centrar el logo horizontalmente
                    float pageWidth = e.PageBounds.Width;
                    float logoX = (pageWidth - newWidth) / 2;
                    if (logoX < 0) logoX = 0;
                    
                    // Dibujar fondo blanco MUY GRANDE para el logo (máxima visibilidad)
                    float padding = 15; // Padding muy grande
                    float bgX = logoX - padding;
                    float bgY = topMargin - padding;
                    float bgWidth = newWidth + (padding * 2);
                    float bgHeight = newHeight + (padding * 2);
                    
                    // Asegurar que el fondo no se salga de los límites
                    if (bgX < 0) bgX = 0;
                    if (bgY < 0) bgY = 0;
                    if (bgX + bgWidth > e.PageBounds.Width) bgWidth = e.PageBounds.Width - bgX;
                    
                    // Fondo NEGRO sólido para máximo contraste
                    graphics.FillRectangle(System.Drawing.Brushes.Black, bgX, bgY, bgWidth, bgHeight);
                    
                    // Borde DORADO muy grueso (5px) para mejor definición y elegancia
                    using (var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 215, 0), 5)) // Color dorado
                    {
                        graphics.DrawRectangle(pen, bgX, bgY, bgWidth, bgHeight);
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"Dibujando logo LOGO.png en posición X: {logoX}, Y: {topMargin}");
                    System.Diagnostics.Debug.WriteLine($"Tamaño del logo: {newWidth}x{newHeight} píxeles");
                    System.Diagnostics.Debug.WriteLine($"Fondo: X={bgX}, Y={bgY}, W={bgWidth}, H={bgHeight}");
                    
                    // Configurar calidad de renderizado para mejor visualización
                    var oldInterpolation = graphics.InterpolationMode;
                    var oldSmoothing = graphics.SmoothingMode;
                    var oldPixelOffset = graphics.PixelOffsetMode;
                    var oldCompositing = graphics.CompositingMode;
                    var oldCompositingQuality = graphics.CompositingQuality;
                    
                    // Usar alta calidad pero compatible con impresoras térmicas
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    
                    // Dibujar el logo sobre el fondo blanco - método más directo
                    var destRect = new System.Drawing.RectangleF(logoX, topMargin, newWidth, newHeight);
                    graphics.DrawImage(logo, destRect);
                    
                    System.Diagnostics.Debug.WriteLine("Logo dibujado exitosamente");
                    
                    // Restaurar configuración anterior
                    graphics.InterpolationMode = oldInterpolation;
                    graphics.SmoothingMode = oldSmoothing;
                    graphics.PixelOffsetMode = oldPixelOffset;
                    graphics.CompositingMode = oldCompositing;
                    graphics.CompositingQuality = oldCompositingQuality;
                    
                    // Ajustar margen superior para el texto que viene después
                    topMargin += bgHeight + 20; // Espacio generoso después del logo grande
                }
                catch (Exception ex)
                {
                    // Si falla, continuar sin logo pero registrar el error
                    System.Diagnostics.Debug.WriteLine($"ERROR al dibujar logo en ticket térmico: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Tipo de excepción: {ex.GetType().Name}");
                    System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                }
                finally
                {
                    // Liberar recursos del logo
                    logo?.Dispose();
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ADVERTENCIA: No se pudo cargar el logo LOGO.png");
            }

            foreach (string linea in lineasImprimir)
            {
                yPos = topMargin + (count * fuente.GetHeight(graphics));
                
                // Determinar fuente y color según el contenido
                Font fuenteActual;
                System.Drawing.Brush brushColor;
                
                if (linea.Contains("TOTAL") || linea.Contains("TALLER RODRIGUEZ") || linea.Contains("SERVICIO AUTOMOTRIZ"))
                {
                    fuenteActual = fuenteNegrita;
                    // Color dorado para texto importante (RGB: 255, 215, 0)
                    brushColor = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 215, 0));
                }
                else
                {
                    fuenteActual = fuente;
                    brushColor = System.Drawing.Brushes.Black;
                }

                graphics.DrawString(linea, fuenteActual, brushColor, leftMargin, yPos, new StringFormat());
                
                // Liberar brush si es el dorado personalizado
                if (brushColor != System.Drawing.Brushes.Black)
                {
                    brushColor.Dispose();
                }
                
                count++;
            }

            // Agregar QR al final si es crédito fiscal
            var factura = ObtenerFacturaActual(); // Necesitamos obtener la factura actual
            if (factura != null && EsCreditoFiscal(factura))
            {
                try
                {
                    var qrData = GenerarDatosQr(factura);
                    var qrImage = _qrService.GenerarQrImage(qrData, 80);
                    
                    // Dibujar QR centrado
                    float qrX = (e.PageBounds.Width - 80) / 2;
                    float qrY = yPos + 20;
                    graphics.DrawImage(qrImage, qrX, qrY, 80, 80);
                }
                catch
                {
                    // Si falla generar QR, continuar sin él
                }
            }

            e.HasMorePages = false;
        }

        private Factura? _facturaActual;
        private void EstablecerFacturaActual(Factura factura)
        {
            _facturaActual = factura;
        }

        private Factura? ObtenerFacturaActual()
        {
            return _facturaActual;
        }

        private void AgregarLinea(string texto)
        {
            lineasImprimir.Add(texto);
        }

        private void AgregarLineaCentrada(string texto)
        {
            // Si el texto es muy largo, dividirlo en múltiples líneas
            if (texto.Length > anchoTicket)
            {
                // Dividir el texto en palabras
                var palabras = texto.Split(' ');
                var lineaActual = "";
                
                foreach (var palabra in palabras)
                {
                    if ((lineaActual + " " + palabra).Length <= anchoTicket)
                    {
                        lineaActual += (lineaActual == "" ? "" : " ") + palabra;
                    }
                    else
                    {
                        if (lineaActual != "")
                        {
                            int espacios = (anchoTicket - lineaActual.Length) / 2;
                            if (espacios > 0)
                            {
                                lineasImprimir.Add(new string(' ', espacios) + lineaActual);
                            }
                            else
                            {
                                lineasImprimir.Add(lineaActual);
                            }
                        }
                        lineaActual = palabra;
                    }
                }
                
                // Agregar la última línea
                if (lineaActual != "")
                {
                    int espacios = (anchoTicket - lineaActual.Length) / 2;
                    if (espacios > 0)
                    {
                        lineasImprimir.Add(new string(' ', espacios) + lineaActual);
                    }
                    else
                    {
                        lineasImprimir.Add(lineaActual);
                    }
                }
            }
            else
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

        private string? ObtenerRutaLogo()
        {
            try
            {
                // Cargar solo LOGO.png
                var logoPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "LOGO.png");
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
    }
}
