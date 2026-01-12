using Fatura.Models.Catalogos;

namespace Fatura.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de marcas.
    /// Define las operaciones de negocio relacionadas con marcas.
    /// </summary>
    public interface IMarcaService
    {
        Task<IEnumerable<Marca>> GetAllAsync();
        Task<Marca?> GetByIdAsync(int id);
        Task<Marca> CreateAsync(Marca marca);
        Task<Marca> UpdateAsync(int id, Marca marca);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
