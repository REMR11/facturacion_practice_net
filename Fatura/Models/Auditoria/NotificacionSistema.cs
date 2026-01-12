using Fatura.Models.Core;
using Fatura.Models.Enums;

namespace Fatura.Models.Auditoria
{
    /// <summary>
    /// Notificaciones de errores y eventos críticos del sistema.
    /// Solo para errores del sistema, no para notificaciones de usuario.
    /// </summary>
    public partial class NotificacionSistema : BaseEntity
    {
        public int IdNotificacion { get; set; }
        
        /// <summary>
        /// Tipo de notificación del sistema.
        /// </summary>
        public TipoNotificacionSistema Tipo { get; set; }
        
        /// <summary>
        /// Título del error o evento.
        /// </summary>
        public string Titulo { get; set; } = null!;
        
        /// <summary>
        /// Mensaje detallado del error o evento.
        /// </summary>
        public string Mensaje { get; set; } = null!;
        
        /// <summary>
        /// Stack trace del error (si aplica).
        /// </summary>
        public string? StackTrace { get; set; }
        
        /// <summary>
        /// Indica si el problema fue resuelto.
        /// </summary>
        public bool Resuelta { get; set; } = false;
        
        /// <summary>
        /// Fecha en que se resolvió el problema (si aplica).
        /// </summary>
        public DateTime? FechaResolucion { get; set; }
    }
}
