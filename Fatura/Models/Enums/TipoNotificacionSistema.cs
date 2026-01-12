namespace Fatura.Models.Enums
{
    /// <summary>
    /// Enumeración para tipos de notificaciones del sistema.
    /// </summary>
    public enum TipoNotificacionSistema
    {
        /// <summary>
        /// Error del sistema.
        /// </summary>
        Error = 0,
        
        /// <summary>
        /// Advertencia del sistema.
        /// </summary>
        Advertencia = 1,
        
        /// <summary>
        /// Error crítico que requiere atención inmediata.
        /// </summary>
        ErrorCritico = 2
    }
}
