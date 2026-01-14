using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fatura.Models.Core;
using Fatura.Models.Catalogos;

namespace Fatura.Models.Facturacion
{
    /// <summary>
    /// Representa un detalle (línea) de una factura.
    /// Cada detalle corresponde a un producto o servicio facturado.
    /// </summary>
    public class DetalleFactura : BaseEntity
    {
        public DetalleFactura()
        {
            Impuestos = new HashSet<DetalleFacturaImpuesto>();
        }

        [Key]
        public int IdDetalleFactura { get; set; }
        
        /// <summary>
        /// ID de la factura a la que pertenece este detalle.
        /// </summary>
        [ForeignKey("Factura")]
        public int IdFactura { get; set; }
        
        /// <summary>
        /// ID del producto facturado.
        /// </summary>
        [ForeignKey("Producto")]
        public int? IdProducto { get; set; }
        
        /// <summary>
        /// Nombre del producto al momento de la facturación.
        /// Se almacena para mantener integridad histórica.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NombreProducto { get; set; } = null!;
        
        /// <summary>
        /// Unidad de medida del producto al momento de la facturación (opcional).
        /// Se almacena para mantener integridad histórica.
        /// </summary>
        [StringLength(20)]
        public string? UnidadMedida { get; set; }
        
        /// <summary>
        /// Cantidad de unidades del producto facturadas.
        /// </summary>
        public int Cantidad { get; set; }
        
        /// <summary>
        /// Precio unitario del producto al momento de la facturación.
        /// 
        /// DECISIÓN DE DISEÑO: Se almacena el precio unitario aquí en lugar de 
        /// referenciar directamente Producto.Precio porque:
        /// 1. Los precios de productos cambian con el tiempo
        /// 2. Necesitamos mantener el precio histórico al momento de la facturación
        /// 3. Garantiza integridad histórica: si el precio del producto cambia,
        ///    la factura emitida mantiene el precio original
        /// 
        /// Ejemplo: Si un producto costaba $10.00 cuando se facturó, pero luego
        /// su precio cambia a $12.00, la factura debe seguir mostrando $10.00.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }
        
        /// <summary>
        /// Descuento aplicado a este detalle (en monto, no porcentaje).
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Descuento { get; set; } = 0;
        
        /// <summary>
        /// Total del detalle calculado como: (Cantidad * PrecioUnitario) - Descuento.
        /// Propiedad calculada que no se almacena en la base de datos.
        /// </summary>
        public decimal Total => (Cantidad * PrecioUnitario) - Descuento;

        /// <summary>
        /// Relación con la factura a la que pertenece este detalle.
        /// </summary>
        public virtual Factura Factura { get; set; } = null!;
        
        /// <summary>
        /// Relación con el producto facturado (opcional, puede ser null si el producto fue eliminado).
        /// </summary>
        public virtual Producto? Producto { get; set; }
        
        /// <summary>
        /// Colección de impuestos aplicados a este detalle.
        /// </summary>
        public virtual ICollection<DetalleFacturaImpuesto> Impuestos { get; set; }
    }
}
