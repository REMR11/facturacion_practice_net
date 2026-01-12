using Fatura.Models.Core;
using Fatura.Models.Facturacion;

namespace Fatura.Models.Catalogos
{
    /// <summary>
    /// Catálogo de tipos de impuestos aplicables en el sistema.
    /// Ejemplos: IVA, ISR, Impuesto Municipal, etc.
    /// </summary>
    public partial class TipoImpuesto : BaseEntity
    {
        public TipoImpuesto()
        {
            ProductoImpuestos = new HashSet<ProductoImpuesto>();
            DetalleFacturaImpuestos = new HashSet<DetalleFacturaImpuesto>();
        }

        public int IdTipoImpuesto { get; set; }
        
        /// <summary>
        /// Nombre del impuesto (ej: "IVA", "ISR").
        /// </summary>
        public string Nombre { get; set; } = null!;
        
        /// <summary>
        /// Descripción detallada del impuesto (opcional).
        /// </summary>
        public string? Descripcion { get; set; }
        
        /// <summary>
        /// Porcentaje del impuesto (ej: 13.00 para 13%).
        /// </summary>
        public decimal Porcentaje { get; set; }
        
        /// <summary>
        /// Indica si este impuesto aplica a productos físicos.
        /// </summary>
        public bool AplicaProductos { get; set; } = true;
        
        /// <summary>
        /// Indica si este impuesto aplica a servicios.
        /// </summary>
        public bool AplicaServicios { get; set; } = true;
        
        /// <summary>
        /// Indica si el tipo de impuesto está activo y disponible para uso.
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Relación muchos a muchos con productos.
        /// </summary>
        public virtual ICollection<ProductoImpuesto> ProductoImpuestos { get; set; }
        
        /// <summary>
        /// Impuestos aplicados en detalles de factura.
        /// </summary>
        public virtual ICollection<DetalleFacturaImpuesto> DetalleFacturaImpuestos { get; set; }
    }
}
