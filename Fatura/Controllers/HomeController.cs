using Fatura.Models;
using Fatura.Models.ViewModels;
using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Fatura.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController>? _logger;
        private readonly IProductoService _productoService;

        public HomeController(ILogger<HomeController> logger, IProductoService productoService)
        {
            _logger = logger;
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.GetAllAsync();
            var productosActivos = productos.Where(p => p.Activo && p.Precio > 0).ToList();
            
            return View(productosActivos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> TestConnection()
        {
            try
            {
                using (var scope = HttpContext.RequestServices.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<xstoreContext>();
                    var canConnect = await context.Database.CanConnectAsync();
                    
                    if (canConnect)
                    {
                        return Ok(new { 
                            success = true, 
                            message = "Conexión exitosa a PostgreSQL",
                            database = context.Database.GetDbConnection().Database
                        });
                    }
                    else
                    {
                        return BadRequest(new { 
                            success = false, 
                            message = "No se pudo conectar a la base de datos" 
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error al conectar", 
                    error = ex.Message 
                });
            }
        }
    }
}
