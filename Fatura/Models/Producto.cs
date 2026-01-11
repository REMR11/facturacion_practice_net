namespace Fatura.Models
{
    /// <summary>
    /// Representa un producto en el catálogo del sistema.
    /// </summary>
    public partial class Producto : BaseEntity
    {
        public Producto()
        {
            DetalleFacturas = new HashSet<DetalleFactura>();
        }

        public int IdProducto { get; set; }
        
        /// <summary>
        /// ID de la marca del producto.
        /// </summary>
        public int? IdMarca { get; set; }
        
        /// <summary>
        /// ID de la categoría del producto.
        /// </summary>
        public int? IdCategoria { get; set; }
        
        /// <summary>
        /// Nombre del producto.
        /// </summary>
        public string NombreProducto { get; set; } = null!;
        
        /// <summary>
        /// Precio actual del producto.
        /// 
        /// NOTA: Este precio puede cambiar con el tiempo. Cuando se crea un DetalleFactura,
        /// se copia el PrecioUnitario desde aquí para mantener el precio histórico.
        /// Ver comentario en DetalleFactura.PrecioUnitario para más detalles.
        /// </summary>
        public decimal? Precio { get; set; }
        
        /// <summary>
        /// Código único del producto (SKU, código de barras, etc.).
        /// </summary>
        public int? Codigo { get; set; }
        
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
        /// Colección de detalles de factura donde se ha vendido este producto.
        /// </summary>
        public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; }
    }
}

