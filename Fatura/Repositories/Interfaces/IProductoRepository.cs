using Fatura.Models.Catalogos;

namespace Fatura.Repositories.Interfaces
{
    /// <summary>
    /// Repositorio específico para operaciones de productos.
    /// Extiende IRepository con métodos especializados.
    /// </summary>
    public interface IProductoRepository : IRepository<Producto>
    {
        /// <summary>
        /// Obtiene un producto con su categoría y marca incluidas.
        /// </summary>
        Task<Producto?> GetWithRelationsAsync(int id);

        /// <summary>
        /// Obtiene productos por categoría.
        /// </summary>
        Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId);

        /// <summary>
        /// Obtiene productos por marca.
        /// </summary>
        Task<IEnumerable<Producto>> GetByMarcaAsync(int marcaId);

        /// <summary>
        /// Obtiene productos con stock bajo (menor al mínimo).
        /// </summary>
        Task<IEnumerable<Producto>> GetProductosConStockBajoAsync();

        /// <summary>
        /// Obtiene productos activos.
        /// </summary>
        Task<IEnumerable<Producto>> GetProductosActivosAsync();
    }
}
