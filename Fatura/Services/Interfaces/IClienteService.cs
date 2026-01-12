using Fatura.Models.Facturacion;

namespace Fatura.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de clientes.
    /// Define las operaciones de negocio relacionadas con clientes.
    /// </summary>
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByIdAsync(int id);
        Task<Cliente> CreateAsync(Cliente cliente);
        Task<Cliente> UpdateAsync(int id, Cliente cliente);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Cliente>> SearchAsync(string searchTerm);
        Task<IEnumerable<Cliente>> GetClientesActivosAsync();
        Task<int> GetTotalFacturasAsync(int clienteId);
        Task<decimal> GetTotalFacturadoAsync(int clienteId);
    }
}
