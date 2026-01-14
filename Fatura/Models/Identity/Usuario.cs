using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fatura.Models.Core;
using Fatura.Models.Facturacion;
using Fatura.Models.Auditoria;

namespace Fatura.Models.Identity
{
    /// <summary>
    /// Representa un usuario del sistema.
    /// Almacena información de autenticación y autorización.
    /// </summary>
    public partial class Usuario : BaseEntity
    {
        [Key]
        public int IdUsuario { get; set; }
        
        /// <summary>
        /// Nombre de usuario para autenticación.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string? NombreUsuario { get; set; }
        
        /// <summary>
        /// Nombre completo del usuario para mostrar en la interfaz.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string? NombreCompleto { get; set; }
        
        /// <summary>
        /// Hash de la contraseña del usuario (BCrypt, Argon2, etc.).
        /// Nunca se almacena la contraseña en texto plano.
        /// </summary>
        [Required]
        [StringLength(500)]
        public string? ContraseñaHash { get; set; }
        
        /// <summary>
        /// Salt utilizado para el hash de la contraseña (si es necesario).
        /// </summary>
        [StringLength(255)]
        public string? Salt { get; set; }
        
        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        [Required]
        [StringLength(100)]
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
        
        /// <summary>
        /// Historial de transacciones realizadas por este usuario.
        /// </summary>
        public virtual ICollection<HistorialTransaccion> HistorialTransacciones { get; set; } = new HashSet<HistorialTransaccion>();
        
        /// <summary>
        /// Configuración del dashboard del usuario (relación 1:1).
        /// </summary>
        public virtual ConfiguracionDashboard? ConfiguracionDashboard { get; set; }
    }
}
