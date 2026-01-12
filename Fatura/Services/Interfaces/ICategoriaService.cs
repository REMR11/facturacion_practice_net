using Fatura.Models.Catalogos;

namespace Fatura.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de categorías.
    /// Define las operaciones de negocio relacionadas con categorías.
    /// </summary>
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> GetAllAsync();
        Task<Categoria?> GetByIdAsync(int id);
        Task<Categoria> CreateAsync(Categoria categoria);
        Task<Categoria> UpdateAsync(int id, Categoria categoria);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
