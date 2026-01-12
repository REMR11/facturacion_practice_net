using Fatura.Exceptions;
using Fatura.Models.Catalogos;
using Fatura.Repositories.Interfaces;
using Fatura.Services.Interfaces;

namespace Fatura.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de marcas.
    /// Contiene la lógica de negocio relacionada con marcas.
    /// </summary>
    public class MarcaService : IMarcaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarcaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Marca>> GetAllAsync()
        {
            return await _unitOfWork.Marcas.GetAllAsync();
        }

        public async Task<Marca?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Marcas.GetByIdAsync(id);
        }

        public async Task<Marca> CreateAsync(Marca marca)
        {
            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(marca.NombreMarca))
            {
                throw new BusinessRuleException("NombreRequerido", "El nombre de la marca es requerido.");
            }

            await _unitOfWork.Marcas.AddAsync(marca);
            await _unitOfWork.SaveChangesAsync();

            return marca;
        }

        public async Task<Marca> UpdateAsync(int id, Marca marca)
        {
            var existingMarca = await _unitOfWork.Marcas.GetByIdAsync(id);
            if (existingMarca == null)
            {
                throw new EntityNotFoundException("Marca", id);
            }

            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(marca.NombreMarca))
            {
                throw new BusinessRuleException("NombreRequerido", "El nombre de la marca es requerido.");
            }

            existingMarca.NombreMarca = marca.NombreMarca;

            await _unitOfWork.Marcas.UpdateAsync(existingMarca);
            await _unitOfWork.SaveChangesAsync();

            return existingMarca;
        }

        public async Task DeleteAsync(int id)
        {
            var marca = await _unitOfWork.Marcas.GetByIdAsync(id);
            if (marca == null)
            {
                throw new EntityNotFoundException("Marca", id);
            }

            // Verificar si la marca tiene productos asociados
            var productosConMarca = await _unitOfWork.Productos.FindAsync(p => p.IdMarca == id);
            if (productosConMarca.Any())
            {
                throw new BusinessRuleException("MarcaEnUso", 
                    "No se puede eliminar la marca porque tiene productos asociados.");
            }

            await _unitOfWork.Marcas.DeleteAsync(marca);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Marcas.ExistsAsync(id);
        }
    }
}
