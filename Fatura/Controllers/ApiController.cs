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

        public ApiController(IClienteService clienteService, IProductoService productoService)
        {
            _clienteService = clienteService;
            _productoService = productoService;
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
    }
}
