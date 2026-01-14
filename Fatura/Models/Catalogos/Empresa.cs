using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fatura.Models.Core;

namespace Fatura.Models.Catalogos
{
    /// <summary>
    /// Información de la empresa que emite las facturas.
    /// Configuración única del sistema.
    /// </summary>
    public partial class Empresa : BaseEntity
    {
        [Key]
        public int IdEmpresa { get; set; }
        
        /// <summary>
        /// Razón social de la empresa.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string RazonSocial { get; set; } = null!;
        
        /// <summary>
        /// Nombre comercial de la empresa (opcional).
        /// </summary>
        [StringLength(200)]
        public string? NombreComercial { get; set; }
        
        /// <summary>
        /// NIT (Número de Identificación Tributaria) de la empresa.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Nit { get; set; } = null!;
        
        /// <summary>
        /// Dirección fiscal de la empresa.
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Direccion { get; set; } = null!;
        
        /// <summary>
        /// Teléfono de contacto de la empresa (opcional).
        /// </summary>
        [StringLength(20)]
        public string? Telefono { get; set; }
        
        /// <summary>
        /// Email de contacto de la empresa (opcional).
        /// </summary>
        [StringLength(100)]
        public string? Email { get; set; }
        
        /// <summary>
        /// URL del sitio web de la empresa (opcional).
        /// </summary>
        [StringLength(200)]
        public string? SitioWeb { get; set; }
        
        /// <summary>
        /// URL o path del logo de la empresa (opcional).
        /// </summary>
        [StringLength(500)]
        public string? LogoUrl { get; set; }
        
        /// <summary>
        /// Régimen fiscal de la empresa (opcional).
        /// </summary>
        [StringLength(100)]
        public string? RegimenFiscal { get; set; }
        
        /// <summary>
        /// Actividad económica principal de la empresa (opcional).
        /// </summary>
        [StringLength(200)]
        public string? ActividadEconomica { get; set; }
    }
}
