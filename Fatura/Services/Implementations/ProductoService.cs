using Fatura.Exceptions;
using Fatura.Models.Catalogos;
using Fatura.Models.Enums;
using Fatura.Repositories.Interfaces;
using Fatura.Services.Interfaces;

namespace Fatura.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de productos.
    /// Contiene la lógica de negocio relacionada con productos.
    /// </summary>
    public class ProductoService : IProductoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _unitOfWork.Productos.GetAllAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Productos.GetByIdAsync(id);
        }

        public async Task<Producto> CreateAsync(Producto producto)
        {
            // Validar que la categoría existe si se proporciona
            if (producto.IdCategoria.HasValue)
            {
                var categoria = await _unitOfWork.Categorias.GetByIdAsync(producto.IdCategoria.Value);
                if (categoria == null)
                {
                    throw new EntityNotFoundException("Categoria", producto.IdCategoria.Value);
                }
            }

            // Validar que la marca existe si se proporciona
            if (producto.IdMarca.HasValue)
            {
                var marca = await _unitOfWork.Marcas.GetByIdAsync(producto.IdMarca.Value);
                if (marca == null)
                {
                    throw new EntityNotFoundException("Marca", producto.IdMarca.Value);
                }
            }

            // Validar precio
            if (producto.Precio.HasValue && producto.Precio.Value < 0)
            {
                throw new BusinessRuleException("PrecioInvalido", "El precio no puede ser negativo.");
            }

            // Validar stock
            if (producto.Stock < 0)
            {
                throw new BusinessRuleException("StockInvalido", "El stock no puede ser negativo.");
            }

            await _unitOfWork.Productos.AddAsync(producto);
            await _unitOfWork.SaveChangesAsync();

            return producto;
        }

        public async Task<Producto> UpdateAsync(int id, Producto producto)
        {
            var existingProducto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (existingProducto == null)
            {
                throw new EntityNotFoundException("Producto", id);
            }

            // Validar que la categoría existe si se proporciona
            if (producto.IdCategoria.HasValue)
            {
                var categoria = await _unitOfWork.Categorias.GetByIdAsync(producto.IdCategoria.Value);
                if (categoria == null)
                {
                    throw new EntityNotFoundException("Categoria", producto.IdCategoria.Value);
                }
            }

            // Validar que la marca existe si se proporciona
            if (producto.IdMarca.HasValue)
            {
                var marca = await _unitOfWork.Marcas.GetByIdAsync(producto.IdMarca.Value);
                if (marca == null)
                {
                    throw new EntityNotFoundException("Marca", producto.IdMarca.Value);
                }
            }

            // Validar precio
            if (producto.Precio.HasValue && producto.Precio.Value < 0)
            {
                throw new BusinessRuleException("PrecioInvalido", "El precio no puede ser negativo.");
            }

            // Actualizar propiedades
            existingProducto.NombreProducto = producto.NombreProducto;
            existingProducto.Descripcion = producto.Descripcion;
            existingProducto.Precio = producto.Precio;
            existingProducto.Codigo = producto.Codigo;
            existingProducto.Stock = producto.Stock;
            existingProducto.StockMinimo = producto.StockMinimo;
            existingProducto.Activo = producto.Activo;
            existingProducto.IdCategoria = producto.IdCategoria;
            existingProducto.IdMarca = producto.IdMarca;
            existingProducto.IdUnidadMedida = producto.IdUnidadMedida;
            existingProducto.Tipo = producto.Tipo;

            await _unitOfWork.Productos.UpdateAsync(existingProducto);
            await _unitOfWork.SaveChangesAsync();

            return existingProducto;
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (producto == null)
            {
                throw new EntityNotFoundException("Producto", id);
            }

            // Verificar si el producto tiene facturas asociadas
            var detallesConProducto = await _unitOfWork.DetalleFacturas.FindAsync(d => d.IdProducto == id);
            if (detallesConProducto.Any())
            {
                throw new BusinessRuleException("ProductoEnUso", 
                    "No se puede eliminar el producto porque tiene facturas asociadas.");
            }

            await _unitOfWork.Productos.DeleteAsync(producto);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Productos.ExistsAsync(id);
        }

        public async Task<IEnumerable<Producto>> GetProductosActivosAsync()
        {
            return await _unitOfWork.Productos.GetProductosActivosAsync();
        }

        public async Task<IEnumerable<Producto>> GetProductosConStockBajoAsync()
        {
            return await _unitOfWork.Productos.GetProductosConStockBajoAsync();
        }

        public async Task<IEnumerable<Producto>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync();
            }

            var term = searchTerm.ToLower();
            var todosProductos = await _unitOfWork.Productos.GetAllAsync();

            return todosProductos.Where(p =>
                p.NombreProducto.ToLower().Contains(term) ||
                (p.Codigo != null && p.Codigo.ToString()!.ToLower().Contains(term)) ||
                (p.Descripcion != null && p.Descripcion.ToLower().Contains(term))
            );
        }

        public async Task<IEnumerable<Producto>> FilterByTipoAsync(TipoProducto? tipo)
        {
            if (tipo == null)
            {
                return await GetAllAsync();
            }

            var todosProductos = await _unitOfWork.Productos.GetAllAsync();
            return todosProductos.Where(p => p.Tipo == tipo);
        }
    }
}
