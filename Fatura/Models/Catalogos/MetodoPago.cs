using Fatura.Models.Core;
using Fatura.Models.Facturacion;

namespace Fatura.Models.Catalogos
{
    /// <summary>
    /// Catálogo de métodos de pago disponibles en el sistema.
    /// Ejemplos: Efectivo, Tarjeta de Crédito, Transferencia Bancaria, etc.
    /// </summary>
    public partial class MetodoPago : BaseEntity
    {
        public MetodoPago()
        {
            Facturas = new HashSet<Factura>();
        }

        public int IdMetodoPago { get; set; }
        
        /// <summary>
        /// Nombre del método de pago (ej: "Efectivo", "Tarjeta de Crédito").
        /// </summary>
        public string Nombre { get; set; } = null!;
        
        /// <summary>
        /// Descripción detallada del método de pago (opcional).
        /// </summary>
        public string? Descripcion { get; set; }
        
        /// <summary>
        /// Indica si este método de pago requiere un número de referencia.
        /// Por ejemplo, transferencias bancarias requieren número de referencia.
        /// </summary>
        public bool RequiereReferencia { get; set; } = false;
        
        /// <summary>
        /// Indica si el método de pago está activo y disponible para uso.
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Colección de facturas que utilizan este método de pago.
        /// </summary>
        public virtual ICollection<Factura> Facturas { get; set; }
    }
}
