using Fatura.Exceptions;
using Fatura.Models.Facturacion;
using Fatura.Repositories.Interfaces;
using Fatura.Services.Interfaces;

namespace Fatura.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de clientes.
    /// Contiene la lógica de negocio relacionada con clientes.
    /// </summary>
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _unitOfWork.Clientes.GetAllAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Clientes.GetByIdAsync(id);
        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                throw new BusinessRuleException("NombreRequerido", "El nombre del cliente es requerido.");
            }

            // Validar que el NIT/DUI no esté vacío
            if (string.IsNullOrWhiteSpace(cliente.NitDui))
            {
                throw new BusinessRuleException("NitDuiRequerido", "El NIT/DUI del cliente es requerido.");
            }

            // Verificar que el NIT/DUI no esté duplicado
            var clienteExistente = await _unitOfWork.Clientes.FindAsync(c => c.NitDui == cliente.NitDui);
            if (clienteExistente.Any())
            {
                throw new BusinessRuleException("NitDuiDuplicado", "Ya existe un cliente con este NIT/DUI.");
            }

            await _unitOfWork.Clientes.AddAsync(cliente);
            await _unitOfWork.SaveChangesAsync();

            return cliente;
        }

        public async Task<Cliente> UpdateAsync(int id, Cliente cliente)
        {
            var existingCliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (existingCliente == null)
            {
                throw new EntityNotFoundException("Cliente", id);
            }

            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                throw new BusinessRuleException("NombreRequerido", "El nombre del cliente es requerido.");
            }

            // Validar que el NIT/DUI no esté vacío
            if (string.IsNullOrWhiteSpace(cliente.NitDui))
            {
                throw new BusinessRuleException("NitDuiRequerido", "El NIT/DUI del cliente es requerido.");
            }

            // Verificar que el NIT/DUI no esté duplicado (excepto el actual)
            var clienteConMismoNit = await _unitOfWork.Clientes.FindAsync(c => c.NitDui == cliente.NitDui && c.Id != id);
            if (clienteConMismoNit.Any())
            {
                throw new BusinessRuleException("NitDuiDuplicado", "Ya existe otro cliente con este NIT/DUI.");
            }

            // Actualizar propiedades
            existingCliente.Nombre = cliente.Nombre;
            existingCliente.NitDui = cliente.NitDui;
            existingCliente.Direccion = cliente.Direccion;
            existingCliente.Telefono = cliente.Telefono;
            existingCliente.Email = cliente.Email;
            existingCliente.Activo = cliente.Activo;

            await _unitOfWork.Clientes.UpdateAsync(existingCliente);
            await _unitOfWork.SaveChangesAsync();

            return existingCliente;
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (cliente == null)
            {
                throw new EntityNotFoundException("Cliente", id);
            }

            // Verificar si el cliente tiene facturas asociadas
            var facturasConCliente = await _unitOfWork.Facturas.GetByClienteAsync(id);
            if (facturasConCliente.Any())
            {
                throw new BusinessRuleException("ClienteEnUso", 
                    "No se puede eliminar el cliente porque tiene facturas asociadas.");
            }

            await _unitOfWork.Clientes.DeleteAsync(cliente);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Clientes.ExistsAsync(id);
        }

        public async Task<IEnumerable<Cliente>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync();
            }

            var term = searchTerm.ToLower();
            var todosClientes = await _unitOfWork.Clientes.GetAllAsync();
            
            return todosClientes.Where(c =>
                c.Nombre.ToLower().Contains(term) ||
                c.NitDui.ToLower().Contains(term) ||
                (c.Email != null && c.Email.ToLower().Contains(term))
            );
        }

        public async Task<IEnumerable<Cliente>> GetClientesActivosAsync()
        {
            return await _unitOfWork.Clientes.FindAsync(c => c.Activo);
        }

        public async Task<int> GetTotalFacturasAsync(int clienteId)
        {
            var facturas = await _unitOfWork.Facturas.GetByClienteAsync(clienteId);
            return facturas.Count();
        }

        public async Task<decimal> GetTotalFacturadoAsync(int clienteId)
        {
            var facturas = await _unitOfWork.Facturas.GetByClienteAsync(clienteId);
            return facturas.Sum(f => f.Total);
        }
    }
}
