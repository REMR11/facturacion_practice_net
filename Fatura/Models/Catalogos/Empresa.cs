using Fatura.Models.Core;

namespace Fatura.Models.Catalogos
{
    /// <summary>
    /// Información de la empresa que emite las facturas.
    /// Configuración única del sistema.
    /// </summary>
    public partial class Empresa : BaseEntity
    {
        public int IdEmpresa { get; set; }
        
        /// <summary>
        /// Razón social de la empresa.
        /// </summary>
        public string RazonSocial { get; set; } = null!;
        
        /// <summary>
        /// Nombre comercial de la empresa (opcional).
        /// </summary>
        public string? NombreComercial { get; set; }
        
        /// <summary>
        /// NIT (Número de Identificación Tributaria) de la empresa.
        /// </summary>
        public string Nit { get; set; } = null!;
        
        /// <summary>
        /// Dirección fiscal de la empresa.
        /// </summary>
        public string Direccion { get; set; } = null!;
        
        /// <summary>
        /// Teléfono de contacto de la empresa (opcional).
        /// </summary>
        public string? Telefono { get; set; }
        
        /// <summary>
        /// Email de contacto de la empresa (opcional).
        /// </summary>
        public string? Email { get; set; }
        
        /// <summary>
        /// URL del sitio web de la empresa (opcional).
        /// </summary>
        public string? SitioWeb { get; set; }
        
        /// <summary>
        /// URL o path del logo de la empresa (opcional).
        /// </summary>
        public string? LogoUrl { get; set; }
        
        /// <summary>
        /// Régimen fiscal de la empresa (opcional).
        /// </summary>
        public string? RegimenFiscal { get; set; }
        
        /// <summary>
        /// Actividad económica principal de la empresa (opcional).
        /// </summary>
        public string? ActividadEconomica { get; set; }
    }
}
