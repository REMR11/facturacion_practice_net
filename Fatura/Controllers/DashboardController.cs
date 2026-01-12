using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Controllers
{
    /// <summary>
    /// Controlador para el dashboard principal de la aplicación.
    /// Proporciona métricas, gráficos y datos resumidos.
    /// </summary>
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Página principal del dashboard.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            ViewBag.IngresosDelMes = await _dashboardService.GetIngresosDelMesAsync();
            ViewBag.CambioIngresos = await _dashboardService.GetCambioIngresosPorcentajeAsync();
            ViewBag.FacturasEmitidas = await _dashboardService.GetFacturasEmitidasMesAsync();
            ViewBag.CambioFacturas = await _dashboardService.GetCambioFacturasPorcentajeAsync();
            ViewBag.ClientesActivos = await _dashboardService.GetClientesActivosAsync();
            ViewBag.CambioClientes = await _dashboardService.GetCambioClientesPorcentajeAsync();
            ViewBag.TasaDeCobro = await _dashboardService.GetTasaDeCobroAsync();
            ViewBag.CambioTasaCobro = await _dashboardService.GetCambioTasaCobroPorcentajeAsync();
            ViewBag.IngresosMensuales = await _dashboardService.GetIngresosMensualesAsync(7);
            ViewBag.FacturasRecientes = await _dashboardService.GetFacturasRecientesAsync(5);

            return View();
        }

        /// <summary>
        /// Endpoint API para obtener métricas del dashboard (para AJAX).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMetricas()
        {
            var metricas = new
            {
                ingresosDelMes = await _dashboardService.GetIngresosDelMesAsync(),
                cambioIngresos = await _dashboardService.GetCambioIngresosPorcentajeAsync(),
                facturasEmitidas = await _dashboardService.GetFacturasEmitidasMesAsync(),
                cambioFacturas = await _dashboardService.GetCambioFacturasPorcentajeAsync(),
                clientesActivos = await _dashboardService.GetClientesActivosAsync(),
                cambioClientes = await _dashboardService.GetCambioClientesPorcentajeAsync(),
                tasaDeCobro = await _dashboardService.GetTasaDeCobroAsync(),
                cambioTasaCobro = await _dashboardService.GetCambioTasaCobroPorcentajeAsync()
            };

            return Json(metricas);
        }

        /// <summary>
        /// Endpoint API para obtener datos del gráfico de ingresos mensuales.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetIngresosMensuales(int meses = 7)
        {
            var datos = await _dashboardService.GetIngresosMensualesAsync(meses);
            return Json(datos);
        }

        /// <summary>
        /// Endpoint API para obtener facturas recientes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFacturasRecientes(int cantidad = 5)
        {
            var facturas = await _dashboardService.GetFacturasRecientesAsync(cantidad);
            return Json(facturas);
        }
    }
}
