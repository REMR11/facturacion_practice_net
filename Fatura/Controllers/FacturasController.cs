using Fatura.Models.Enums;
using Fatura.Models.Facturacion;
using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        public FacturasController(
            IFacturaService facturaService, 
            IProductoService productoService,
            IClienteService clienteService)
        {
            _facturaService = facturaService;
            _productoService = productoService;
            _clienteService = clienteService;
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
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var factura = await _facturaService.GetWithDetailsAsync(id);
                return View(factura);
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
            if (id == null)
            {
                var factura = new Factura
                {
                    Total = 0,
                    SubTotal = 0,
                    Iva = 0,
                    Isr = 0,
                    OtrosImpuestos = 0,
                    FechaCreacion = DateTime.UtcNow
                };

                ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();

                return View(factura);
            }
            else
            {
                ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                var factura = await _facturaService.GetWithDetailsAsync(id.Value);
                ViewBag.DetalleFacturas = factura.DetalleFacturas;
                return View(factura);
            }
        }

        /// <summary>
        /// Crea una nueva factura.
        /// </summary>
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Factura factura)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _facturaService.CreateAsync(factura);
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                return View(factura);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear la factura: {ex.Message}");
                ViewBag.Productos = await _productoService.GetProductosActivosAsync();
                ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                return View(factura);
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
                ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                return View(factura);
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
                ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                return View(factura);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar la factura: {ex.Message}");
                ViewBag.Clientes = await _clienteService.GetClientesActivosAsync();
                return View(factura);
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
