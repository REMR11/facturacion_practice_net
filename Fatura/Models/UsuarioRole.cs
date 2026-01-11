namespace Fatura.Models
{
    /// <summary>
    /// Tabla intermedia para la relación muchos a muchos entre Usuario y Role.
    /// Permite que un usuario tenga múltiples roles y un rol pueda estar asignado a múltiples usuarios.
    /// </summary>
    public class UsuarioRole
    {
        /// <summary>
        /// ID del usuario.
        /// </summary>
        public int UsuarioId { get; set; }
        
        /// <summary>
        /// ID del rol.
        /// </summary>
        public int RoleId { get; set; }
        
        /// <summary>
        /// Fecha y hora en que se asignó el rol al usuario.
        /// </summary>
        public DateTime AsignadoEn { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Relación con el usuario.
        /// </summary>
        public virtual Usuario Usuario { get; set; } = null!;
        
        /// <summary>
        /// Relación con el rol.
        /// </summary>
        public virtual Role Role { get; set; } = null!;
    }
}
