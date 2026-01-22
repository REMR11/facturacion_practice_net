using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fatura.Models.Core;
using Fatura.Models.Enums;
using Fatura.Models.Identity;
using Fatura.Models.Catalogos;
using Fatura.Models.Auditoria;

namespace Fatura.Models.Facturacion
{
    /// <summary>
    /// Representa una factura en el sistema.
    /// Contiene información fiscal, del cliente y los detalles de los productos facturados.
    /// </summary>
    public partial class Factura : BaseEntity
    {
        public Factura()
        {
            DetalleFacturas = new HashSet<DetalleFactura>();
            HistorialTransacciones = new HashSet<HistorialTransaccion>();
        }

        [Key]
        public int IdFactura { get; set; }
        
        /// <summary>
        /// Número único de factura generado por el sistema.
        /// Formato recomendado: FAC-YYYY-NNNN (ej: FAC-2024-0001)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string NumeroFactura { get; set; } = null!;
        
        /// <summary>
        /// Serie de facturación fiscal asignada por la autoridad tributaria.
        /// Requerido para facturas con validez fiscal.
        /// </summary>
        [StringLength(20)]
        public string? SerieFactura { get; set; }
        
        /// <summary>
        /// Código de Autorización de Impresión (CAI) o Documento Tributario Electrónico (DTE).
        /// Autorización fiscal requerida para la emisión de facturas.
        /// </summary>
        [StringLength(100)]
        public string? CaiDte { get; set; }
        
        /// <summary>
        /// Fecha límite de emisión del CAI/DTE.
        /// La factura debe emitirse antes de esta fecha para mantener validez fiscal.
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? FechaLimiteEmision { get; set; }
        
        /// <summary>
        /// Fecha de creación/emisión de la factura.
        /// </summary>
        public DateTime FechaCreacion { get; set; }
        
        /// <summary>
        /// Fecha de vencimiento para el pago de la factura.
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? FechaVencimiento { get; set; }
        
        /// <summary>
        /// Tipo de documento fiscal (Factura, Nota de Crédito, Nota de Débito, etc.).
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TipoDocumento { get; set; } = "Factura";
        
        /// <summary>
        /// ID del cliente al que se emite la factura.
        /// Relación con la tabla Cliente.
        /// </summary>
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        
        /// <summary>
        /// NIT/DUI del cliente al momento de la facturación.
        /// Se almacena aquí para mantener integridad histórica (el NIT del cliente puede cambiar).
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ClienteNitDui { get; set; } = null!;
        
        /// <summary>
        /// Nombre del cliente al momento de la facturación.
        /// Se almacena aquí para mantener integridad histórica (el nombre del cliente puede cambiar).
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ClienteNombre { get; set; } = null!;
        
        /// <summary>
        /// Dirección del cliente al momento de la facturación (opcional).
        /// Se almacena aquí para mantener integridad histórica.
        /// </summary>
        [StringLength(500)]
        public string? ClienteDireccion { get; set; }
        
        /// <summary>
        /// Correo electrónico del cliente al momento de la facturación (opcional).
        /// Se almacena aquí para mantener integridad histórica y permitir el envío de facturas.
        /// </summary>
        [StringLength(100)]
        public string? ClienteEmail { get; set; }
        
        /// <summary>
        /// Subtotal de la factura antes de impuestos.
        /// Suma de todos los detalles sin incluir impuestos.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }
        
        /// <summary>
        /// Monto total de Impuesto al Valor Agregado (IVA).
        /// Calculado sobre el subtotal según la tasa vigente.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Iva { get; set; }
        
        /// <summary>
        /// Monto total de Impuesto Sobre la Renta (ISR) u otros impuestos.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Isr { get; set; }
        
        /// <summary>
        /// Otros impuestos o cargos adicionales.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal OtrosImpuestos { get; set; }
        
        /// <summary>
        /// Total general de la factura incluyendo todos los impuestos.
        /// Calculado como: SubTotal + Iva + Isr + OtrosImpuestos
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        
        /// <summary>
        /// Estado actual de la factura (Borrador, Pendiente, Pagada, Cancelada).
        /// </summary>
        public EstadoFactura Estado { get; set; } = EstadoFactura.Borrador;
        
        /// <summary>
        /// ID del usuario que creó/emitió la factura.
        /// Relación con la tabla Usuario.
        /// </summary>
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        
        /// <summary>
        /// ID del método de pago utilizado (opcional).
        /// </summary>
        [ForeignKey("MetodoPago")]
        public int? IdMetodoPago { get; set; }
        
        /// <summary>
        /// Número de referencia del método de pago (opcional).
        /// Por ejemplo, número de transacción bancaria.
        /// </summary>
        [StringLength(100)]
        public string? ReferenciaMetodoPago { get; set; }
        
        /// <summary>
        /// Fecha en que se realizó el pago (opcional).
        /// </summary>
        public DateTime? FechaPago { get; set; }
        
        /// <summary>
        /// Símbolo de la moneda (siempre USD: "$").
        /// </summary>
        [Required]
        [StringLength(5)]
        public string MonedaSimbolo { get; set; } = "$";
        
        /// <summary>
        /// Relación con el cliente.
        /// </summary>
        public virtual Cliente Cliente { get; set; } = null!;
        
        /// <summary>
        /// Relación con el usuario que emitió la factura.
        /// </summary>
        public virtual Usuario Usuario { get; set; } = null!;
        
        /// <summary>
        /// Relación con el método de pago utilizado.
        /// </summary>
        public virtual MetodoPago? MetodoPago { get; set; }
        
        /// <summary>
        /// Colección de detalles (productos) de la factura.
        /// </summary>
        public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; }
        
        /// <summary>
        /// Historial de transacciones relacionadas con esta factura.
        /// </summary>
        public virtual ICollection<HistorialTransaccion> HistorialTransacciones { get; set; }
    }
}
