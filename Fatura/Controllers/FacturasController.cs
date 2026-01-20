using Fatura.Models;
using Fatura.Models.Enums;
using Fatura.Models.Facturacion;
using Fatura.Models.ViewModels;
using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        private readonly xstoreContext _context;

        public FacturasController(
            IFacturaService facturaService, 
            IProductoService productoService,
            IClienteService clienteService,
            IFacturaPdfService facturaPdfService,
             IFacturaTicketService facturaTicketService,
            xstoreContext context)
        {
            _facturaService = facturaService;
            _productoService = productoService;
            _clienteService = clienteService;
            _facturaPdfService = facturaPdfService;
            _facturaTicketService = facturaTicketService;
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
        /// Genera el PDF de una factura.
        /// </summary>
        [HttpGet("{id}/Pdf")]
        public async Task<IActionResult> Pdf(int id)
        {
            try
            {
                var factura = await _facturaService.GetWithDetailsAsync(id);
                var pdfBytes = _facturaPdfService.GenerarPdf(factura);
                var fileName = $"Factura_{factura.NumeroFactura ?? id.ToString()}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Imprime el ticket térmico de una factura directamente en la impresora RPT004.
        /// </summary>
        [HttpGet("{id}/Ticket")]
        public async Task<IActionResult> Ticket(int id, string? printer = null)
        {
            try
            {
                var factura = await _facturaService.GetWithDetailsAsync(id);
                var nombreImpresora = printer ?? "RPT004";
                
                bool impreso = _facturaTicketService.ImprimirTicket(factura, nombreImpresora);
                
                if (impreso)
                {
                    TempData["Success"] = $"Ticket impreso correctamente en {nombreImpresora}";
                }
                else
                {
                    TempData["Error"] = "No se pudo imprimir el ticket";
                }
                
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al imprimir: {ex.Message}";
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
                var pdfBytes = _facturaTicketService.GenerarTicket(factura);
                var fileName = $"Ticket_{factura.NumeroFactura ?? id.ToString()}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
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
                    return View(model);
                }

                var cliente = await _clienteService.GetByIdAsync(model.ClienteId);
                if (cliente == null)
                {
                    ModelState.AddModelError("", "Cliente no encontrado.");
                    ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                    ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                    return View(model);
                }

                var items = model.Items?.Where(i => i.IdProducto > 0).ToList() ?? new List<FacturaItemCreateViewModel>();
                if (items.Count == 0)
                {
                    ModelState.AddModelError("", "Agrega al menos un producto válido.");
                    ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                    ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                    return View(model);
                }

                var detalles = new List<DetalleFactura>();
                foreach (var item in items)
                {
                    var producto = await _productoService.GetByIdAsync(item.IdProducto);
                    if (producto == null)
                    {
                        ModelState.AddModelError("", $"Producto no encontrado: {item.IdProducto}");
                        ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                        ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                        return View(model);
                    }

                    detalles.Add(new DetalleFactura
                    {
                        IdProducto = producto.IdProducto,
                        NombreProducto = producto.NombreProducto,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = producto.Precio ?? 0,
                        Descuento = item.Descuento
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
                    return View(model);
                }

                var factura = new Factura
                {
                    ClienteId = cliente.Id,
                    ClienteNitDui = cliente.NitDui,
                    ClienteNombre = cliente.Nombre,
                    ClienteDireccion = cliente.Direccion,
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
                return RedirectToAction(nameof(Details), new { id = creada.IdFactura, download = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear la factura: {ex.Message}");
                ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
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
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al eliminar la factura: {ex.Message}");
                return View();
            }
        }
    }
}
