using Fatura.Models.Facturacion;

namespace Fatura.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio del dashboard.
    /// Define las operaciones para obtener métricas y datos agregados.
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Obtiene los ingresos del mes actual.
        /// </summary>
        Task<decimal> GetIngresosDelMesAsync();

        /// <summary>
        /// Obtiene el porcentaje de cambio de ingresos vs mes anterior.
        /// </summary>
        Task<decimal> GetCambioIngresosPorcentajeAsync();

        /// <summary>
        /// Obtiene el número de facturas emitidas en el mes actual.
        /// </summary>
        Task<int> GetFacturasEmitidasMesAsync();

        /// <summary>
        /// Obtiene el porcentaje de cambio de facturas emitidas vs mes anterior.
        /// </summary>
        Task<decimal> GetCambioFacturasPorcentajeAsync();

        /// <summary>
        /// Obtiene el número de clientes activos.
        /// </summary>
        Task<int> GetClientesActivosAsync();

        /// <summary>
        /// Obtiene el porcentaje de cambio de clientes activos vs mes anterior.
        /// </summary>
        Task<decimal> GetCambioClientesPorcentajeAsync();

        /// <summary>
        /// Obtiene la tasa de cobro (porcentaje de facturas pagadas).
        /// </summary>
        Task<decimal> GetTasaDeCobroAsync();

        /// <summary>
        /// Obtiene el porcentaje de cambio de tasa de cobro vs mes anterior.
        /// </summary>
        Task<decimal> GetCambioTasaCobroPorcentajeAsync();

        /// <summary>
        /// Obtiene los ingresos mensuales de los últimos N meses.
        /// </summary>
        Task<Dictionary<string, decimal>> GetIngresosMensualesAsync(int meses = 7);

        /// <summary>
        /// Obtiene las últimas N facturas del sistema.
        /// </summary>
        Task<IEnumerable<Factura>> GetFacturasRecientesAsync(int cantidad = 5);
    }
}
