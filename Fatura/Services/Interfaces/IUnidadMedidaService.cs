using Fatura.Models.Catalogos;

namespace Fatura.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de unidades de medida.
    /// </summary>
    public interface IUnidadMedidaService
    {
        Task<IEnumerable<UnidadMedida>> GetAllAsync();
        Task<UnidadMedida?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
