using Fatura.Models.Catalogos;
using Fatura.Models.Facturacion;

namespace Fatura.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para el patrón Unit of Work.
    /// Coordina múltiples repositorios y maneja transacciones.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Categoria> Categorias { get; }
        IProductoRepository Productos { get; }
        IRepository<Marca> Marcas { get; }
        IRepository<UnidadMedida> UnidadesMedida { get; }
        IFacturaRepository Facturas { get; }
        IRepository<Cliente> Clientes { get; }
        IRepository<DetalleFactura> DetalleFacturas { get; }
        
        /// <summary>
        /// Guarda todos los cambios realizados en los repositorios.
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Inicia una transacción.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Confirma la transacción actual.
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Revierte la transacción actual.
        /// </summary>
        Task RollbackTransactionAsync();
    }
}
