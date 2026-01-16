using Fatura.Models.Catalogos;
using Fatura.Repositories.Interfaces;
using Fatura.Services.Interfaces;

namespace Fatura.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de unidades de medida.
    /// </summary>
    public class UnidadMedidaService : IUnidadMedidaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnidadMedidaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UnidadMedida>> GetAllAsync()
        {
            return await _unitOfWork.UnidadesMedida.GetAllAsync();
        }

        public async Task<UnidadMedida?> GetByIdAsync(int id)
        {
            return await _unitOfWork.UnidadesMedida.GetByIdAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.UnidadesMedida.ExistsAsync(id);
        }
    }
}
