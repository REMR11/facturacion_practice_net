namespace Fatura.Models
{
    /// <summary>
    /// Clase base abstracta que proporciona campos de auditoría comunes a todas las entidades.
    /// Implementa el patrón de soft delete y trazabilidad de cambios.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Fecha y hora de creación del registro.
        /// Se establece automáticamente al crear la entidad.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Fecha y hora de la última actualización del registro.
        /// Se actualiza automáticamente cuando se modifica la entidad.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// ID del usuario que creó el registro.
        /// Permite auditoría de quién creó cada entidad.
        /// </summary>
        public int? CreatedBy { get; set; }
        
        /// <summary>
        /// ID del usuario que realizó la última actualización.
        /// Permite auditoría de quién modificó cada entidad.
        /// </summary>
        public int? UpdatedBy { get; set; }
        
        /// <summary>
        /// Indica si el registro ha sido eliminado lógicamente (soft delete).
        /// Cuando es true, el registro no se muestra en consultas normales pero se mantiene en la BD.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
        
        /// <summary>
        /// Fecha y hora en que se realizó la eliminación lógica.
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        
        /// <summary>
        /// ID del usuario que realizó la eliminación lógica.
        /// Permite auditoría de quién eliminó cada entidad.
        /// </summary>
        public int? DeletedBy { get; set; }
    }
}
