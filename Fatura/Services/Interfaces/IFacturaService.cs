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
    }
}
