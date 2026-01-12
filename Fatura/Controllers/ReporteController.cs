using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Controllers
{
    public class ReporteController : Controller
    {
        private readonly IReporteService _reporteService;

        public ReporteController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<FileResult> ExportarProductos(DateTime fechainicio, DateTime fechafin)
        {
            try
            {
                var stream = await _reporteService.ExportarProductosAsync(fechainicio, fechafin);
                return File(stream.ToArray(), 
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                    $"Reporte Recepciones {DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                // En producción, deberías loggear el error
                throw new Exception($"Error al generar el reporte: {ex.Message}", ex);
            }
        }
    }
}
