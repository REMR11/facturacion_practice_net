using System.Linq.Expressions;

namespace Fatura.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz genérica para operaciones CRUD básicas en repositorios.
    /// Proporciona métodos comunes para todas las entidades.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las entidades.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Busca entidades que cumplan con el predicado especificado.
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Agrega una nueva entidad.
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Elimina una entidad.
        /// </summary>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Verifica si una entidad existe por su ID.
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Cuenta el número de entidades que cumplen con el predicado (opcional).
        /// </summary>
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    }
}
