using Fatura.Models.Enums;
using Fatura.Models.Facturacion;

namespace Fatura.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de facturas.
    /// Define las operaciones de negocio relacionadas con facturas.
    /// </summary>
    public interface IFacturaService
    {
        Task<IEnumerable<Factura>> GetAllAsync();
        Task<Factura?> GetByIdAsync(int id);
        Task<Factura> CreateAsync(Factura factura);
        Task<Factura> UpdateAsync(int id, Factura factura);
        Task DeleteAsync(int id);
        Task<DetalleFactura> AgregarProductoAsync(int facturaId, int productoId, int cantidad = 1);
        Task CalcularTotalAsync(int facturaId);
        Task<Factura> GetWithDetailsAsync(int id);
        Task<IEnumerable<Factura>> SearchAsync(string searchTerm);
        Task<IEnumerable<Factura>> FilterByEstadoAsync(EstadoFactura? estado);
        Task<IEnumerable<Factura>> FilterByFechaAsync(DateTime? fechaInicio, DateTime? fechaFin);
        Task<(IEnumerable<Factura> facturas, int total)> GetPagedAsync(int page, int pageSize, string? searchTerm = null, EstadoFactura? estado = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);
    }
}
