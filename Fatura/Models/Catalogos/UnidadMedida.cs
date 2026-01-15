using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fatura.Models.Core;

namespace Fatura.Models.Catalogos
{
    /// <summary>
    /// Catálogo de unidades de medida para productos y servicios.
    /// Ejemplos: Hora, Unidad, Mes, Sesión, Kilogramo, etc.
    /// </summary>
    public partial class UnidadMedida : BaseEntity
    {
        public UnidadMedida()
        {
            Productos = new HashSet<Producto>();
        }

        [Key]
        public int IdUnidadMedida { get; set; }
        
        /// <summary>
        /// Nombre de la unidad de medida (ej: "Hora", "Unidad", "Mes").
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = null!;
        
        /// <summary>
        /// Abreviatura de la unidad (ej: "hr", "u", "mes").
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Abreviatura { get; set; } = null!;
        
        /// <summary>
        /// Descripción detallada de la unidad de medida (opcional).
        /// </summary>
        [StringLength(200)]
        public string? Descripcion { get; set; }
        
        /// <summary>
        /// Indica si la unidad de medida está activa y disponible para uso.
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Colección de productos que utilizan esta unidad de medida.
        /// </summary>
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
