namespace Fatura.Models
{
    /// <summary>
    /// Representa un rol en el sistema de autorización.
    /// Los roles definen los permisos y acceso de los usuarios.
    /// </summary>
    public class Role : BaseEntity
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Nombre del rol (ej: Administrador, Vendedor, Contador).
        /// </summary>
        public string Nombre { get; set; } = null!;
        
        /// <summary>
        /// Descripción del rol y sus permisos.
        /// </summary>
        public string? Descripcion { get; set; }
        
        /// <summary>
        /// Colección de usuarios que tienen este rol asignado.
        /// </summary>
        public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new HashSet<UsuarioRole>();
    }
}
