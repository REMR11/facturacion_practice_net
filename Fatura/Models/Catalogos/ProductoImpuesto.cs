using Fatura.Models.Core;

namespace Fatura.Models.Catalogos
{
    /// <summary>
    /// Tabla de relación muchos a muchos entre Producto y TipoImpuesto.
    /// Permite asignar múltiples impuestos a un producto y viceversa.
    /// </summary>
    public partial class ProductoImpuesto : BaseEntity
    {
        /// <summary>
        /// ID del producto al que se aplica el impuesto.
        /// </summary>
        public int IdProducto { get; set; }
        
        /// <summary>
        /// ID del tipo de impuesto aplicado.
        /// </summary>
        public int IdTipoImpuesto { get; set; }
        
        /// <summary>
        /// Porcentaje personalizado para este producto específico.
        /// Si es null, se usa el porcentaje del TipoImpuesto.
        /// Permite sobrescribir el porcentaje estándar para casos especiales.
        /// </summary>
        public decimal? PorcentajePersonalizado { get; set; }

        /// <summary>
        /// Relación con el producto.
        /// </summary>
        public virtual Producto Producto { get; set; } = null!;
        
        /// <summary>
        /// Relación con el tipo de impuesto.
        /// </summary>
        public virtual TipoImpuesto TipoImpuesto { get; set; } = null!;
    }
}
