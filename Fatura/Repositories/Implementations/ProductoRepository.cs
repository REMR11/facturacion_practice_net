using Fatura.Models;
using Fatura.Models.Catalogos;
using Fatura.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fatura.Repositories.Implementations
{
    /// <summary>
    /// Implementación del repositorio específico para productos.
    /// Proporciona métodos especializados para consultas complejas de productos.
    /// </summary>
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        public ProductoRepository(xstoreContext context) : base(context)
        {
        }

        public async Task<Producto?> GetWithRelationsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.UnidadMedida)
                .FirstOrDefaultAsync(p => p.IdProducto == id);
        }

        public async Task<IEnumerable<Producto>> GetByCategoriaAsync(int categoriaId)
        {
            return await _dbSet
                .Where(p => p.IdCategoria == categoriaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetByMarcaAsync(int marcaId)
        {
            return await _dbSet
                .Where(p => p.IdMarca == marcaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetProductosConStockBajoAsync()
        {
            return await _dbSet
                .Where(p => p.Stock <= p.StockMinimo && p.Activo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetProductosActivosAsync()
        {
            return await _dbSet
                .Where(p => p.Activo)
                .ToListAsync();
        }
    }
}
