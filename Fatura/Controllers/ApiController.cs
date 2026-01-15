using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IProductoService _productoService;
        private readonly IFacturaService _facturaService;

        public ApiController(
            IClienteService clienteService,
            IProductoService productoService,
            IFacturaService facturaService)
        {
            _clienteService = clienteService;
            _productoService = productoService;
            _facturaService = facturaService;
        }

        [HttpGet("clientes")]
        public async Task<IActionResult> GetClientes()
        {
            try
            {
                var clientes = await _clienteService.GetClientesActivosAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("productos")]
        public async Task<IActionResult> GetProductos()
        {
            try
            {
                var productos = await _productoService.GetProductosActivosAsync();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GlobalSearch([FromQuery] string term, [FromQuery] int limit = 5)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Ok(new { results = Array.Empty<object>() });
            }

            var facturas = await _facturaService.SearchAsync(term);
            var clientes = await _clienteService.SearchAsync(term);
            var productos = await _productoService.SearchAsync(term);

            var resultados = new List<object>();

            resultados.AddRange(facturas
                .Take(limit)
                .Select(f => new
                {
                    type = "Factura",
                    title = $"Factura {f.NumeroFactura}",
                    subtitle = $"{f.ClienteNombre} · {f.Total:C}",
                    url = $"/Facturas/{f.IdFactura}"
                }));

            resultados.AddRange(clientes
                .Take(limit)
                .Select(c => new
                {
                    type = "Cliente",
                    title = c.Nombre,
                    subtitle = c.NitDui,
                    url = $"/Clientes/Details?id={c.Id}"
                }));

            resultados.AddRange(productos
                .Take(limit)
                .Select(p => new
                {
                    type = "Producto",
                    title = p.NombreProducto,
                    subtitle = p.Codigo.HasValue ? $"Código {p.Codigo}" : "Sin código",
                    url = $"/Productos/{p.IdProducto}"
                }));

            return Ok(new { results = resultados });
        }
    }
}
