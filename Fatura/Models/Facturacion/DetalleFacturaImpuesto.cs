using Fatura.Models.Core;
using Fatura.Models.Catalogos;

namespace Fatura.Models.Facturacion
{
    /// <summary>
    /// Almacena los impuestos aplicados a cada detalle de factura.
    /// Mantiene el histórico de impuestos al momento de la facturación.
    /// </summary>
    public partial class DetalleFacturaImpuesto : BaseEntity
    {
        public int IdDetalleFacturaImpuesto { get; set; }
        
        /// <summary>
        /// ID del detalle de factura al que se aplica este impuesto.
        /// </summary>
        public int IdDetalleFactura { get; set; }
        
        /// <summary>
        /// ID del tipo de impuesto aplicado.
        /// </summary>
        public int IdTipoImpuesto { get; set; }
        
        /// <summary>
        /// Nombre del impuesto al momento de la facturación.
        /// Se almacena para mantener integridad histórica.
        /// </summary>
        public string NombreImpuesto { get; set; } = null!;
        
        /// <summary>
        /// Porcentaje del impuesto aplicado al momento de la facturación.
        /// Se almacena para mantener integridad histórica.
        /// </summary>
        public decimal Porcentaje { get; set; }
        
        /// <summary>
        /// Base imponible sobre la que se calcula el impuesto.
        /// Generalmente: Cantidad * PrecioUnitario - Descuento
        /// </summary>
        public decimal BaseImponible { get; set; }
        
        /// <summary>
        /// Monto calculado del impuesto.
        /// Calculado como: BaseImponible * (Porcentaje / 100)
        /// </summary>
        public decimal MontoImpuesto { get; set; }

        /// <summary>
        /// Relación con el detalle de factura.
        /// </summary>
        public virtual DetalleFactura DetalleFactura { get; set; } = null!;
        
        /// <summary>
        /// Relación con el tipo de impuesto.
        /// </summary>
        public virtual TipoImpuesto TipoImpuesto { get; set; } = null!;
    }
}
