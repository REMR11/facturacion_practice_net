using Fatura.Models.Core;
using Fatura.Models.Enums;
using Fatura.Models.Facturacion;
using Fatura.Models.Identity;

namespace Fatura.Models.Auditoria
{
    /// <summary>
    /// Registro de auditoría de todas las transacciones del sistema.
    /// Permite rastrear quién hizo qué, cuándo y con qué monto.
    /// </summary>
    public partial class HistorialTransaccion : BaseEntity
    {
        public int IdHistorial { get; set; }
        
        /// <summary>
        /// ID de la factura relacionada (si aplica).
        /// </summary>
        public int? IdFactura { get; set; }
        
        /// <summary>
        /// ID del usuario que realizó la transacción.
        /// </summary>
        public int IdUsuarioEmisor { get; set; }
        
        /// <summary>
        /// ID del cliente receptor de la factura (si aplica).
        /// </summary>
        public int? IdCliente { get; set; }
        
        /// <summary>
        /// Tipo de transacción realizada.
        /// </summary>
        public TipoTransaccion TipoTransaccion { get; set; }
        
        /// <summary>
        /// Descripción detallada de la transacción.
        /// </summary>
        public string Descripcion { get; set; } = null!;
        
        /// <summary>
        /// Monto de la transacción (si aplica).
        /// </summary>
        public decimal? Monto { get; set; }
        
        /// <summary>
        /// Fecha y hora en que se realizó la transacción.
        /// </summary>
        public DateTime FechaTransaccion { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Datos adicionales en formato JSON (opcional).
        /// Permite almacenar información extra específica de cada tipo de transacción.
        /// </summary>
        public string? DatosAdicionales { get; set; }

        /// <summary>
        /// Relación con la factura (si aplica).
        /// </summary>
        public virtual Factura? Factura { get; set; }
        
        /// <summary>
        /// Relación con el usuario que realizó la acción.
        /// </summary>
        public virtual Usuario UsuarioEmisor { get; set; } = null!;
        
        /// <summary>
        /// Relación con el cliente (si aplica).
        /// </summary>
        public virtual Cliente? Cliente { get; set; }
    }
}
