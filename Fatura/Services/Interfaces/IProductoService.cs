using Fatura.Models.Catalogos;
using Fatura.Models.Enums;

namespace Fatura.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de productos.
    /// Define las operaciones de negocio relacionadas con productos.
    /// </summary>
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<Producto?> GetByIdAsync(int id);
        Task<Producto> CreateAsync(Producto producto);
        Task<Producto> UpdateAsync(int id, Producto producto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Producto>> GetProductosActivosAsync();
        Task<IEnumerable<Producto>> GetProductosConStockBajoAsync();
        Task<IEnumerable<Producto>> SearchAsync(string searchTerm);
        Task<IEnumerable<Producto>> FilterByTipoAsync(TipoProducto? tipo);
    }
}
