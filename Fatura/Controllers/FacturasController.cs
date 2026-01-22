using Fatura.Models;
using Fatura.Models.Enums;
using Fatura.Models.Facturacion;
using Fatura.Models.ViewModels;
using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fatura.Controllers
{
    /// <summary>
    /// Controlador para gestionar facturas.
    /// Proporciona funcionalidad para listar, buscar, filtrar, crear, editar y eliminar facturas.
    /// </summary>
    [Route("Facturas")]
    public class FacturasController : Controller
    {
        private readonly IFacturaService _facturaService;
        private readonly IProductoService _productoService;
        private readonly IClienteService _clienteService;
        private readonly IFacturaPdfService _facturaPdfService;
        private readonly IFacturaTicketService _facturaTicketService;
        private readonly IEmailService _emailService;

        private readonly xstoreContext _context;

        public FacturasController(
            IFacturaService facturaService, 
            IProductoService productoService,
            IClienteService clienteService,
            IFacturaPdfService facturaPdfService,
             IFacturaTicketService facturaTicketService,
            IEmailService emailService,
            xstoreContext context)
        {
            _facturaService = facturaService;
            _productoService = productoService;
            _clienteService = clienteService;
            _facturaPdfService = facturaPdfService;
            _facturaTicketService = facturaTicketService;
            _emailService = emailService;
            _context = context;
        }




        /// <summary>
        /// Lista todas las facturas con opciones de búsqueda, filtros y paginación.
        /// </summary>
        public async Task<IActionResult> Index(
            string? searchTerm = null,
            EstadoFactura? estado = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int page = 1,
            int pageSize = 10)
        {
            var (facturas, total) = await _facturaService.GetPagedAsync(page, pageSize, searchTerm, estado, fechaInicio, fechaFin);

            ViewBag.SearchTerm = searchTerm;
            ViewBag.Estado = estado;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);

            return View(facturas);
        }

        /// <summary>
        /// Endpoint API para búsqueda de facturas (para AJAX).
        /// </summary>
        [HttpGet("api/search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                var todas = await _facturaService.GetAllAsync();
                return Json(todas);
            }

            var resultados = await _facturaService.SearchAsync(term);
            return Json(resultados);
        }

        /// <summary>
        /// Endpoint API para obtener facturas paginadas (para AJAX).
        /// </summary>
        [HttpGet("api/list")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] EstadoFactura? estado = null,
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null)
        {
            var (facturas, total) = await _facturaService.GetPagedAsync(page, pageSize, searchTerm, estado, fechaInicio, fechaFin);

            return Json(new
            {
                facturas,
                total,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            });
        }

        /// <summary>
        /// Obtiene los detalles de una factura.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id, bool download = false)
        {
            try
            {
                var factura = await _facturaService.GetWithDetailsAsync(id);
                ViewBag.AutoDownload = download;
                return View(factura);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Genera el PDF de una factura y lo envía por correo al cliente si tiene email configurado.
        /// </summary>
        [HttpGet("{id}/Pdf")]
        public async Task<IActionResult> Pdf(int id)
        {
            try
            {
                var factura = await _facturaService.GetWithDetailsAsync(id);
                if (factura == null)
                {
                    TempData["Error"] = "Factura no encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                System.Diagnostics.Debug.WriteLine($"Generando PDF para factura ID: {id}");

                var pdfBytes = _facturaPdfService.GenerarPdf(factura);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    TempData["Error"] = "Error al generar el PDF de la factura: el documento está vacío.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                var fileName = $"Factura_{factura.NumeroFactura ?? id.ToString()}.pdf";
                
                // Intentar enviar el correo al cliente si tiene email
                var emailCliente = factura.ClienteEmail;
                if (string.IsNullOrWhiteSpace(emailCliente) && factura.Cliente != null)
                {
                    emailCliente = factura.Cliente.Email;
                }

                if (!string.IsNullOrWhiteSpace(emailCliente) && emailCliente.Contains("@"))
                {
                    try
                    {
                        var asunto = $"Factura {factura.NumeroFactura} - {factura.ClienteNombre}";
                        var cuerpo = $@"
                            <html>
                            <body style='font-family: Arial, sans-serif;'>
                                <h2 style='color: #333;'>Estimado/a {factura.ClienteNombre},</h2>
                                <p>Adjuntamos la factura <strong>{factura.NumeroFactura}</strong> correspondiente a su compra.</p>
                                <p><strong>Fecha de emisión:</strong> {factura.FechaCreacion:dd/MM/yyyy}</p>
                                <p><strong>Total:</strong> {factura.MonedaSimbolo} {factura.Total:N2}</p>
                                {(factura.FechaVencimiento.HasValue ? $"<p><strong>Fecha de vencimiento:</strong> {factura.FechaVencimiento.Value:dd/MM/yyyy}</p>" : "")}
                                <p>Por favor, conserve este documento para sus registros.</p>
                                <p>Saludos cordiales,<br/>Sistema de Facturación</p>
                            </body>
                            </html>";

                        var correoEnviado = await _emailService.EnviarCorreoConAdjuntoAsync(
                            emailCliente,
                            asunto,
                            cuerpo,
                            pdfBytes,
                            fileName
                        );

                        if (correoEnviado)
                        {
                            TempData["Success"] = $"PDF generado y enviado exitosamente a {emailCliente}";
                        }
                        else
                        {
                            TempData["Warning"] = $"PDF generado correctamente, pero no se pudo enviar el correo a {emailCliente}. Verifique la configuración (Gmail: use contraseña de aplicación en https://myaccount.google.com/apppasswords).";
                        }
                    }
                    catch (Exception emailEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al enviar correo: {emailEx.Message}");
                        TempData["Warning"] = $"PDF generado correctamente, pero error al enviar el correo: {emailEx.Message} Si usas Gmail, necesitas contraseña de aplicación: https://myaccount.google.com/apppasswords";
                    }
                }
                
                // Configurar headers para mostrar el PDF en el navegador
                Response.Headers.Add("Content-Disposition", $"inline; filename=\"{fileName}\"");
                
                System.Diagnostics.Debug.WriteLine($"PDF generado exitosamente. Tamaño: {pdfBytes.Length} bytes. Archivo: {fileName}");
                
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                TempData["Error"] = "Factura no encontrada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log del error completo para debugging
                System.Diagnostics.Debug.WriteLine($"ERROR en Pdf action: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException.Message}");
                }
                
                TempData["Error"] = $"Error al generar el PDF: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        /// <summary>
        /// Genera el PDF de la factura y lo envía por correo al email configurado (sin descargar).
        /// Redirige a Details con mensaje de éxito o error.
        /// </summary>
        [HttpGet("{id}/EnviarPdf")]
        public async Task<IActionResult> EnviarPdf(int id)
        {
            try
            {
                var factura = await _facturaService.GetWithDetailsAsync(id);
                if (factura == null)
                {
                    TempData["Error"] = "Factura no encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                var emailCliente = factura.ClienteEmail;
                if (string.IsNullOrWhiteSpace(emailCliente) && factura.Cliente != null)
                    emailCliente = factura.Cliente.Email;

                if (string.IsNullOrWhiteSpace(emailCliente) || !emailCliente.Contains("@"))
                {
                    TempData["Error"] = "No hay correo configurado para esta factura. Agregue un email al cliente o en los datos de la factura.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                var pdfBytes = _facturaPdfService.GenerarPdf(factura);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    TempData["Error"] = "Error al generar el PDF de la factura.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                var fileName = $"Factura_{factura.NumeroFactura ?? id.ToString()}.pdf";
                var asunto = $"Factura {factura.NumeroFactura} - {factura.ClienteNombre}";
                var cuerpo = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h2 style='color: #333;'>Estimado/a {factura.ClienteNombre},</h2>
                        <p>Adjuntamos la factura <strong>{factura.NumeroFactura}</strong> correspondiente a su compra.</p>
                        <p><strong>Fecha de emisión:</strong> {factura.FechaCreacion:dd/MM/yyyy}</p>
                        <p><strong>Total:</strong> {factura.MonedaSimbolo} {factura.Total:N2}</p>
                        {(factura.FechaVencimiento.HasValue ? $"<p><strong>Fecha de vencimiento:</strong> {factura.FechaVencimiento.Value:dd/MM/yyyy}</p>" : "")}
                        <p>Por favor, conserve este documento para sus registros.</p>
                        <p>Saludos cordiales,<br/>Sistema de Facturación</p>
                    </body>
                    </html>";

                var correoEnviado = await _emailService.EnviarCorreoConAdjuntoAsync(
                    emailCliente, asunto, cuerpo, pdfBytes, fileName);

                if (correoEnviado)
                    TempData["Success"] = $"PDF enviado correctamente a {emailCliente}.";
                else
                    TempData["Warning"] = $"No se pudo enviar el correo a {emailCliente}. Verifique Email en appsettings o User Secrets. Gmail: use contraseña de aplicación (https://myaccount.google.com/apppasswords).";

                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                TempData["Error"] = "Factura no encontrada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error EnviarPdf: {ex.Message}");
                TempData["Error"] = $"Error al enviar el PDF por correo: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        /// <summary>
        /// Imprime el ticket térmico de una factura directamente en la impresora RPT004.
        /// En Linux/Azure, redirige automáticamente a generar el PDF del ticket.
        /// </summary>
        [HttpGet("{id}/Ticket")]
        public async Task<IActionResult> Ticket(int id, string? printer = null)
        {
            try
            {
                var factura = await _facturaService.GetWithDetailsAsync(id);
                if (factura == null)
                {
                    TempData["Error"] = "Factura no encontrada.";
                    return RedirectToAction(nameof(Index));
                }
                
                var nombreImpresora = printer ?? "RPT004";
                
                // El método ahora lanza excepciones en lugar de retornar false
                _facturaTicketService.ImprimirTicket(factura, nombreImpresora);
                
                TempData["Success"] = $"Ticket impreso correctamente en {nombreImpresora}";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (PlatformNotSupportedException ex)
            {
                // En Linux/Azure, generar el PDF del ticket automáticamente
                try
                {
                    var factura = await _facturaService.GetWithDetailsAsync(id);
                    if (factura != null)
                    {
                        TempData["Info"] = "La impresión directa no está disponible en este servidor. Se generará el PDF del ticket para que pueda imprimirlo manualmente.";
                        return RedirectToAction(nameof(TicketPdf), new { id });
                    }
                }
                catch
                {
                    // Si falla generar PDF, mostrar el mensaje de error
                }
                
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                TempData["Error"] = "Factura no encontrada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Mostrar el mensaje de error completo al usuario
                var mensajeError = ex.Message;
                if (ex.InnerException != null)
                {
                    mensajeError += $" Detalles: {ex.InnerException.Message}";
                }
                TempData["Error"] = mensajeError;
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        /// <summary>
        /// Genera el PDF del ticket térmico (para preview o descarga).
        /// </summary>
        [HttpGet("{id}/TicketPdf")]
        public async Task<IActionResult> TicketPdf(int id)
        {
            try
            {
                var factura = await _facturaService.GetWithDetailsAsync(id);
                if (factura == null)
                {
                    TempData["Error"] = "Factura no encontrada.";
                    return RedirectToAction(nameof(Index));
                }
                
                var pdfBytes = _facturaTicketService.GenerarTicket(factura);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    TempData["Error"] = "Error al generar el PDF del ticket: el documento está vacío.";
                    return RedirectToAction(nameof(Details), new { id });
                }
                
                var fileName = $"Ticket_{factura.NumeroFactura ?? id.ToString()}.pdf";
                
                // Configurar headers para mostrar el PDF en el navegador en lugar de descargarlo
                Response.Headers.Add("Content-Disposition", $"inline; filename=\"{fileName}\"");
                
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                TempData["Error"] = "Factura no encontrada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log del error completo para debugging
                System.Diagnostics.Debug.WriteLine($"ERROR en TicketPdf: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException.Message}");
                }
                
                TempData["Error"] = $"Error al generar el PDF del ticket: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        /// <summary>
        /// Actualiza el estado de una factura (Pendiente, Pagada, Cancelada, etc.).
        /// </summary>
        [HttpPost("{id}/Estado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEstado(int id, EstadoFactura estado, string? returnUrl = null)
        {
            try
            {
                await _facturaService.UpdateEstadoAsync(id, estado);
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Muestra el formulario para crear una nueva factura.
        /// </summary>
        [HttpGet("Create")]
        public async Task<IActionResult> Create(int? id)
        {
            var viewModel = new FacturaCreateViewModel
            {
                FechaEmision = DateTime.Today,
                FechaVencimiento = DateTime.Today.AddDays(30),
                MonedaSimbolo = "S/",
                TipoDocumento = "01",
                SerieFactura = "F001"
            };

            if (id.HasValue)
            {
                var factura = await _facturaService.GetWithDetailsAsync(id.Value);
                viewModel.ClienteId = factura.ClienteId;
                viewModel.TipoDocumento = factura.TipoDocumento;
                viewModel.SerieFactura = factura.SerieFactura;
                viewModel.FechaEmision = factura.FechaCreacion;
                viewModel.FechaVencimiento = factura.FechaVencimiento;
                viewModel.MonedaSimbolo = factura.MonedaSimbolo;
                viewModel.Items = factura.DetalleFacturas
                    .Where(d => d.IdProducto.HasValue)
                    .Select(d => new FacturaItemCreateViewModel
                    {
                        IdProducto = d.IdProducto!.Value,
                        Cantidad = d.Cantidad,
                        Descuento = d.Descuento,
                        PrecioUnitario = d.PrecioUnitario
                    })
                    .ToList();
            }

            ViewBag.Productos = await _productoService.GetProductosActivosAsync();
            ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();

            return View(viewModel);
        }

        /// <summary>
        /// Crea una nueva factura.
        /// </summary>
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FacturaCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                    ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                    TempData["Error"] = "Por favor, corrija los errores en el formulario.";
                    return View(model);
                }

                // Validar cliente
                if (model.ClienteId <= 0)
                {
                    ModelState.AddModelError("ClienteId", "Debe seleccionar un cliente.");
                    ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                    ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                    TempData["Error"] = "Debe seleccionar un cliente.";
                    return View(model);
                }

                var cliente = await _clienteService.GetByIdAsync(model.ClienteId);
                if (cliente == null)
                {
                    ModelState.AddModelError("ClienteId", "Cliente no encontrado.");
                    ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                    ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                    TempData["Error"] = "Cliente no encontrado.";
                    return View(model);
                }

                // Validar productos
                var items = model.Items?.Where(i => i.IdProducto > 0 && i.Cantidad > 0).ToList() ?? new List<FacturaItemCreateViewModel>();
                if (items.Count == 0)
                {
                    ModelState.AddModelError("", "Debe agregar al menos un producto válido con cantidad mayor a cero.");
                    ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                    ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                    TempData["Error"] = "Debe agregar al menos un producto válido con cantidad mayor a cero.";
                    return View(model);
                }

                var detalles = new List<DetalleFactura>();
                foreach (var item in items)
                {
                    var producto = await _productoService.GetByIdAsync(item.IdProducto);
                    if (producto == null)
                    {
                        ModelState.AddModelError("", $"Producto con ID {item.IdProducto} no encontrado.");
                        ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                        ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                        TempData["Error"] = $"Producto con ID {item.IdProducto} no encontrado.";
                        return View(model);
                    }

                    // Validar cantidad
                    if (item.Cantidad <= 0)
                    {
                        ModelState.AddModelError("", $"La cantidad del producto {producto.NombreProducto} debe ser mayor a cero.");
                        ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                        ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                        TempData["Error"] = $"La cantidad del producto {producto.NombreProducto} debe ser mayor a cero.";
                        return View(model);
                    }

                    var precioUnitario = item.PrecioUnitario > 0 ? item.PrecioUnitario : (producto.Precio ?? 0);
                    if (precioUnitario <= 0)
                    {
                        ModelState.AddModelError("", $"El precio del producto {producto.NombreProducto} debe ser mayor a cero.");
                        ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                        ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                        TempData["Error"] = $"El precio del producto {producto.NombreProducto} debe ser mayor a cero.";
                        return View(model);
                    }

                    // No establecer IdFactura aquí - Entity Framework lo asignará automáticamente
                    // cuando la factura se guarde
                    detalles.Add(new DetalleFactura
                    {
                        IdProducto = producto.IdProducto,
                        NombreProducto = producto.NombreProducto,
                        UnidadMedida = producto.UnidadMedida?.Abreviatura,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = precioUnitario,
                        Descuento = item.Descuento >= 0 ? item.Descuento : 0
                        // IdFactura se establecerá automáticamente por Entity Framework
                    });
                }

                var usuarioId = await _context.Usuarios
                    .Where(u => u.Activo)
                    .Select(u => u.IdUsuario)
                    .FirstOrDefaultAsync();

                if (usuarioId == 0)
                {
                    ModelState.AddModelError("", "No hay usuarios activos para asignar a la factura.");
                    ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                    ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                    TempData["Error"] = "No hay usuarios activos para asignar a la factura.";
                    return View(model);
                }

                // Validar campos requeridos del cliente
                if (string.IsNullOrWhiteSpace(cliente.NitDui))
                {
                    ModelState.AddModelError("", "El cliente no tiene un NIT/DUI válido.");
                    ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                    ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                    TempData["Error"] = "El cliente no tiene un NIT/DUI válido.";
                    return View(model);
                }

                if (string.IsNullOrWhiteSpace(cliente.Nombre))
                {
                    ModelState.AddModelError("", "El cliente no tiene un nombre válido.");
                    ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                    ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                    TempData["Error"] = "El cliente no tiene un nombre válido.";
                    return View(model);
                }

                // Validar campos requeridos de la factura
                if (string.IsNullOrWhiteSpace(model.TipoDocumento))
                {
                    model.TipoDocumento = "Factura Electrónica";
                }

                if (string.IsNullOrWhiteSpace(model.MonedaSimbolo))
                {
                    model.MonedaSimbolo = "$";
                }

                // Obtener el email del formulario (puede venir del campo ClienteEmail o del cliente)
                var clienteEmail = Request.Form["ClienteEmail"].ToString().Trim();
                if (string.IsNullOrWhiteSpace(clienteEmail))
                {
                    clienteEmail = cliente.Email;
                }

                var factura = new Factura
                {
                    ClienteId = cliente.Id,
                    ClienteNitDui = cliente.NitDui,
                    ClienteNombre = cliente.Nombre,
                    ClienteDireccion = cliente.Direccion,
                    ClienteEmail = clienteEmail,
                    TipoDocumento = model.TipoDocumento,
                    SerieFactura = model.SerieFactura,
                    FechaCreacion = model.FechaEmision ?? DateTime.UtcNow,
                    FechaVencimiento = model.FechaVencimiento,
                    MonedaSimbolo = model.MonedaSimbolo,
                    Estado = EstadoFactura.Pendiente,
                    UsuarioId = usuarioId,
                    DetalleFacturas = detalles
                };

                var subTotal = detalles.Sum(d => d.Total);
                var iva = subTotal * 0.13m;
                factura.SubTotal = subTotal;
                factura.Iva = iva;
                factura.Isr = 0;
                factura.OtrosImpuestos = 0;
                factura.Total = subTotal + iva;

                var creada = await _facturaService.CreateAsync(factura);
                TempData["Success"] = $"Factura #{creada.IdFactura} creada exitosamente.";
                return RedirectToAction(nameof(Details), new { id = creada.IdFactura, download = true });
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                TempData["Error"] = ex.Message;
                return View(model);
            }
            catch (DbUpdateException dbEx)
            {
                // Capturar la excepción interna para obtener más detalles
                var errorMessage = dbEx.Message;
                if (dbEx.InnerException != null)
                {
                    errorMessage += $" | Detalles: {dbEx.InnerException.Message}";
                    
                    // Si hay más excepciones internas, incluirlas también
                    if (dbEx.InnerException.InnerException != null)
                    {
                        errorMessage += $" | Más detalles: {dbEx.InnerException.InnerException.Message}";
                    }
                }
                
                // Verificar si es un error de clave duplicada
                if (errorMessage.Contains("duplicate key") || errorMessage.Contains("UNIQUE constraint") || errorMessage.Contains("uk_factura_numero_factura"))
                {
                    errorMessage = "El número de factura ya existe. Por favor, intente nuevamente.";
                }
                // Verificar si es un error de clave foránea
                else if (errorMessage.Contains("foreign key") || errorMessage.Contains("FK_") || errorMessage.Contains("fk_"))
                {
                    errorMessage = "Error de referencia: Verifique que el cliente y usuario existan y estén activos.";
                }
                // Verificar si es un error de campo requerido
                else if (errorMessage.Contains("NOT NULL") || errorMessage.Contains("required") || errorMessage.Contains("cannot be null"))
                {
                    errorMessage = "Faltan campos requeridos. Por favor, verifique todos los datos de la factura.";
                }
                
                ModelState.AddModelError("", errorMessage);
                ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                TempData["Error"] = errorMessage;
                return View(model);
            }
            catch (Exception ex)
            {
                // Capturar la excepción interna si existe
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $" | Detalles: {ex.InnerException.Message}";
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += $" | Más detalles: {ex.InnerException.InnerException.Message}";
                    }
                }
                
                ModelState.AddModelError("", $"Error al crear la factura: {errorMessage}");
                ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                TempData["Error"] = $"Error al crear la factura: {errorMessage}";
                return View(model);
            }
        }

        /// <summary>
        /// Agrega un producto a una factura.
        /// </summary>
        [HttpPost("AgregarProducto")]
        public async Task<IActionResult> AgregarProducto(int idProducto, int idFactura)
        {
            try
            {
                await _facturaService.AgregarProductoAsync(idFactura, idProducto);
                return RedirectToAction(nameof(Create), new { id = idFactura });
            }
            catch (Fatura.Exceptions.EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Create), new { id = idFactura });
            }
        }

        /// <summary>
        /// Muestra el formulario para editar una factura.
        /// </summary>
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var factura = await _facturaService.GetByIdAsync(id);
                if (factura == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Actualiza una factura existente.
        /// </summary>
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Factura factura)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _facturaService.UpdateAsync(id, factura);
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar la factura: {ex.Message}");
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        /// <summary>
        /// Muestra la confirmación para eliminar una factura.
        /// </summary>
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var factura = await _facturaService.GetByIdAsync(id);
                if (factura == null)
                {
                    return NotFound();
                }
                return View(factura);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Elimina una factura.
        /// </summary>
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _facturaService.DeleteAsync(id);
                TempData["Success"] = "Factura eliminada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                TempData["Error"] = "La factura no fue encontrada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar la factura: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
