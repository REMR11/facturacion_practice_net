namespace Fatura.Models
{
    /// <summary>
    /// Representa un usuario del sistema.
    /// Almacena información de autenticación y autorización.
    /// </summary>
    public partial class Usuario : BaseEntity
    {
        public int IdUsuario { get; set; }
        
        /// <summary>
        /// Nombre de usuario para autenticación.
        /// </summary>
        public string? NombreUsuario { get; set; }
        
        /// <summary>
        /// Hash de la contraseña del usuario (BCrypt, Argon2, etc.).
        /// Nunca se almacena la contraseña en texto plano.
        /// </summary>
        public string? ContraseñaHash { get; set; }
        
        /// <summary>
        /// Salt utilizado para el hash de la contraseña (si es necesario).
        /// </summary>
        public string? Salt { get; set; }
        
        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string? Email { get; set; }
        
        /// <summary>
        /// Indica si el usuario está activo y puede acceder al sistema.
        /// </summary>
        public bool Activo { get; set; } = true;
        
        /// <summary>
        /// Fecha y hora del último acceso al sistema.
        /// </summary>
        public DateTime? UltimoAcceso { get; set; }
        
        /// <summary>
        /// Colección de facturas emitidas por este usuario.
        /// </summary>
        public virtual ICollection<Factura> Facturas { get; set; } = new HashSet<Factura>();
        
        /// <summary>
        /// Colección de roles asignados al usuario (relación muchos a muchos).
        /// </summary>
        public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new HashSet<UsuarioRole>();
    }
}
