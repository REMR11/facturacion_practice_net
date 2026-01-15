using Fatura.Models;
using Fatura.Models.Facturacion;
using Fatura.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fatura.Repositories.Implementations
{
    /// <summary>
    /// Implementación del repositorio específico para facturas.
    /// Proporciona métodos especializados para consultas complejas de facturas.
    /// </summary>
    public class FacturaRepository : Repository<Factura>, IFacturaRepository
    {
        public FacturaRepository(xstoreContext context) : base(context)
        {
        }

        public async Task<Factura?> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(f => f.DetalleFacturas)
                .FirstOrDefaultAsync(f => f.IdFactura == id);
        }

        public async Task<IEnumerable<Factura>> GetByClienteAsync(int clienteId)
        {
            return await _dbSet
                .Where(f => f.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Factura>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _dbSet
                .Where(f => f.FechaCreacion >= fechaInicio && f.FechaCreacion <= fechaFin)
                .ToListAsync();
        }

        public async Task<Factura?> GetWithDetailsAndProductsAsync(int id)
        {
            return await _dbSet
                .Include(f => f.DetalleFacturas)
                    .ThenInclude(d => d.Producto)
                .Include(f => f.Cliente)
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(f => f.IdFactura == id);
        }
    }
}
