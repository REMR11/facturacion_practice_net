using Fatura.Models.Core;

namespace Fatura.Models.Catalogos
{
    /// <summary>
    /// Representa una marca de productos.
    /// Permite agrupar productos por su marca comercial.
    /// </summary>
    public partial class Marca : BaseEntity
    {
        public Marca()
        {
            Productos = new HashSet<Producto>();
        }

        public int IdMarca { get; set; }
        
        /// <summary>
        /// Nombre de la marca.
        /// </summary>
        public string NombreMarca { get; set; } = null!;

        /// <summary>
        /// Colección de productos que pertenecen a esta marca.
        /// </summary>
        public virtual ICollection<Producto> Productos { get; set; }
    }
}

