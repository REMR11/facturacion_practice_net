using Fatura.Exceptions;
using Fatura.Models.Catalogos;
using Fatura.Repositories.Interfaces;
using Fatura.Services.Interfaces;

namespace Fatura.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de categorías.
    /// Contiene la lógica de negocio relacionada con categorías.
    /// </summary>
    public class CategoriaService : ICategoriaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            return await _unitOfWork.Categorias.GetAllAsync();
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Categorias.GetByIdAsync(id);
        }

        public async Task<Categoria> CreateAsync(Categoria categoria)
        {
            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(categoria.NombreCategoria))
            {
                throw new BusinessRuleException("NombreRequerido", "El nombre de la categoría es requerido.");
            }

            await _unitOfWork.Categorias.AddAsync(categoria);
            await _unitOfWork.SaveChangesAsync();

            return categoria;
        }

        public async Task<Categoria> UpdateAsync(int id, Categoria categoria)
        {
            var existingCategoria = await _unitOfWork.Categorias.GetByIdAsync(id);
            if (existingCategoria == null)
            {
                throw new EntityNotFoundException("Categoria", id);
            }

            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(categoria.NombreCategoria))
            {
                throw new BusinessRuleException("NombreRequerido", "El nombre de la categoría es requerido.");
            }

            existingCategoria.NombreCategoria = categoria.NombreCategoria;

            await _unitOfWork.Categorias.UpdateAsync(existingCategoria);
            await _unitOfWork.SaveChangesAsync();

            return existingCategoria;
        }

        public async Task DeleteAsync(int id)
        {
            var categoria = await _unitOfWork.Categorias.GetByIdAsync(id);
            if (categoria == null)
            {
                throw new EntityNotFoundException("Categoria", id);
            }

            // Verificar si la categoría tiene productos asociados
            var productosConCategoria = await _unitOfWork.Productos.FindAsync(p => p.IdCategoria == id);
            if (productosConCategoria.Any())
            {
                throw new BusinessRuleException("CategoriaEnUso", 
                    "No se puede eliminar la categoría porque tiene productos asociados.");
            }

            await _unitOfWork.Categorias.DeleteAsync(categoria);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Categorias.ExistsAsync(id);
        }
    }
}
