using Fatura.Models.Facturacion;

namespace Fatura.Repositories.Interfaces
{
    /// <summary>
    /// Repositorio específico para operaciones de facturas.
    /// Extiende IRepository con métodos especializados.
    /// </summary>
    public interface IFacturaRepository : IRepository<Factura>
    {
        /// <summary>
        /// Obtiene una factura con todos sus detalles incluidos.
        /// </summary>
        Task<Factura?> GetWithDetailsAsync(int id);

        /// <summary>
        /// Obtiene todas las facturas de un cliente específico.
        /// </summary>
        Task<IEnumerable<Factura>> GetByClienteAsync(int clienteId);

        /// <summary>
        /// Obtiene facturas en un rango de fechas.
        /// </summary>
        Task<IEnumerable<Factura>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene una factura con detalles y productos incluidos.
        /// </summary>
        Task<Factura?> GetWithDetailsAndProductsAsync(int id);
    }
}
