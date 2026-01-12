namespace Fatura.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de reportes.
    /// Define las operaciones de generación de reportes.
    /// </summary>
    public interface IReporteService
    {
        /// <summary>
        /// Exporta un reporte de productos vendidos en un rango de fechas.
        /// </summary>
        Task<MemoryStream> ExportarProductosAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}
