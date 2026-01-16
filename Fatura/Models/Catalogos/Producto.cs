using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fatura.Models.Core;
using Fatura.Models.Enums;
using Fatura.Models.Facturacion;

namespace Fatura.Models.Catalogos
{
    /// <summary>
    /// Representa un producto en el catálogo del sistema.
    /// </summary>
    public partial class Producto : BaseEntity
    {
        public Producto()
        {
            DetalleFacturas = new HashSet<DetalleFactura>();
            ProductoImpuestos = new HashSet<ProductoImpuesto>();
        }

        [Key]
        public int IdProducto { get; set; }
        
        /// <summary>
        /// Tipo de producto: Producto físico o Servicio.
        /// </summary>
        public TipoProducto Tipo { get; set; } = TipoProducto.Producto;
        
        /// <summary>
        /// ID de la marca del producto.
        /// </summary>
        [ForeignKey("Marca")]
        public int? IdMarca { get; set; }
        
        /// <summary>
        /// ID de la categoría del producto.
        /// </summary>
        [ForeignKey("Categoria")]
        public int? IdCategoria { get; set; }
        
        /// <summary>
        /// ID de la unidad de medida del producto/servicio.
        /// </summary>
        [Required]
        [ForeignKey("UnidadMedida")]
        public int? IdUnidadMedida { get; set; }
        
        /// <summary>
        /// Nombre del producto.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NombreProducto { get; set; } = null!;
        
        /// <summary>
        /// Descripción detallada del producto o servicio.
        /// </summary>
        [StringLength(500)]
        public string? Descripcion { get; set; }
        
        /// <summary>
        /// Precio actual del producto.
        /// 
        /// NOTA: Este precio puede cambiar con el tiempo. Cuando se crea un DetalleFactura,
        /// se copia el PrecioUnitario desde aquí para mantener el precio histórico.
        /// Ver comentario en DetalleFactura.PrecioUnitario para más detalles.
        /// </summary>
        [Required]
        [Range(0, 99999999.99)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Precio { get; set; }
        
        /// <summary>
        /// Código único del producto (SKU, código de barras, etc.).
        /// </summary>
        [Required]
        [StringLength(50)]
        public string? Codigo { get; set; }
        
        /// <summary>
        /// Cantidad disponible en inventario.
        /// </summary>
        public int Stock { get; set; } = 0;
        
        /// <summary>
        /// Stock mínimo requerido antes de generar alerta de reposición.
        /// </summary>
        public int StockMinimo { get; set; } = 0;
        
        /// <summary>
        /// Indica si el producto está activo y disponible para venta.
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Relación con la categoría del producto.
        /// </summary>
        public virtual Categoria? Categoria { get; set; }
        
        /// <summary>
        /// Relación con la marca del producto.
        /// </summary>
        public virtual Marca? Marca { get; set; }
        
        /// <summary>
        /// Relación con la unidad de medida del producto/servicio.
        /// </summary>
        public virtual UnidadMedida? UnidadMedida { get; set; }
        
        /// <summary>
        /// Colección de detalles de factura donde se ha vendido este producto.
        /// </summary>
        public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; }
        
        /// <summary>
        /// Colección de impuestos aplicables a este producto (relación muchos a muchos).
        /// </summary>
        public virtual ICollection<ProductoImpuesto> ProductoImpuestos { get; set; }
    }
}

