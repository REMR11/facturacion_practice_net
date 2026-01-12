using Fatura.Models.Core;
using Fatura.Models.Auditoria;

namespace Fatura.Models.Facturacion
{
    /// <summary>
    /// Representa un cliente del sistema de facturación.
    /// Almacena información relevante del cliente para la emisión de facturas.
    /// </summary>
    public class Cliente : BaseEntity
    {
        public Cliente()
        {
            Facturas = new HashSet<Factura>();
            HistorialTransacciones = new HashSet<HistorialTransaccion>();
        }

        public int Id { get; set; }
        
        /// <summary>
        /// Número de identificación tributaria (NIT) o Documento Único de Identidad (DUI).
        /// Identificador único del cliente ante las autoridades fiscales.
        /// </summary>
        public string NitDui { get; set; } = null!;
        
        /// <summary>
        /// Nombre completo o razón social del cliente.
        /// </summary>
        public string Nombre { get; set; } = null!;
        
        /// <summary>
        /// Dirección física del cliente (opcional).
        /// Útil para facturas que requieren información de entrega o facturación.
        /// </summary>
        public string? Direccion { get; set; }
        
        /// <summary>
        /// Número de teléfono de contacto del cliente.
        /// </summary>
        public string? Telefono { get; set; }
        
        /// <summary>
        /// Correo electrónico del cliente.
        /// </summary>
        public string? Email { get; set; }
        
        /// <summary>
        /// Indica si el cliente está activo en el sistema.
        /// Permite realizar soft delete sin eliminar el registro.
        /// </summary>
        public bool Activo { get; set; } = true;
        
        /// <summary>
        /// Relación con las facturas emitidas a este cliente.
        /// </summary>
        public virtual ICollection<Factura> Facturas { get; set; } = new HashSet<Factura>();
        
        /// <summary>
        /// Historial de transacciones relacionadas con este cliente.
        /// </summary>
        public virtual ICollection<HistorialTransaccion> HistorialTransacciones { get; set; } = new HashSet<HistorialTransaccion>();
    }
}
