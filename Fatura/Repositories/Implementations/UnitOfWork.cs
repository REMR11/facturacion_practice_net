using Fatura.Models;
using Fatura.Models.Catalogos;
using Fatura.Models.Facturacion;
using Fatura.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fatura.Repositories.Implementations
{
    /// <summary>
    /// Implementación del patrón Unit of Work.
    /// Coordina múltiples repositorios y maneja transacciones de base de datos.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly xstoreContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;

        private IRepository<Categoria>? _categorias;
        private IProductoRepository? _productos;
        private IRepository<Marca>? _marcas;
        private IFacturaRepository? _facturas;
        private IRepository<Cliente>? _clientes;
        private IRepository<DetalleFactura>? _detalleFacturas;

        public UnitOfWork(xstoreContext context)
        {
            _context = context;
        }

        public IRepository<Categoria> Categorias
        {
            get
            {
                _categorias ??= new Repository<Categoria>(_context);
                return _categorias;
            }
        }

        public IProductoRepository Productos
        {
            get
            {
                if (_productos == null)
                {
                    _productos = new ProductoRepository(_context);
                }
                return _productos;
            }
        }

        public IRepository<Marca> Marcas
        {
            get
            {
                _marcas ??= new Repository<Marca>(_context);
                return _marcas;
            }
        }

        public IFacturaRepository Facturas
        {
            get
            {
                _facturas ??= new FacturaRepository(_context);
                return _facturas;
            }
        }

        public IRepository<Cliente> Clientes
        {
            get
            {
                _clientes ??= new Repository<Cliente>(_context);
                return _clientes;
            }
        }

        public IRepository<DetalleFactura> DetalleFacturas
        {
            get
            {
                _detalleFacturas ??= new Repository<DetalleFactura>(_context);
                return _detalleFacturas;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
