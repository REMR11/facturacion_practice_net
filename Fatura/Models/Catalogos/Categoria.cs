using Fatura.Models.Core;

namespace Fatura.Models.Catalogos
{
    /// <summary>
    /// Representa una categoría de productos.
    /// Permite organizar y clasificar los productos del catálogo.
    /// </summary>
    public partial class Categoria : BaseEntity
    {
        public Categoria()
        {
            Productos = new HashSet<Producto>();
        }

        public int IdCategoria { get; set; }
        
        /// <summary>
        /// Nombre de la categoría.
        /// </summary>
        public string NombreCategoria { get; set; } = null!;

        /// <summary>
        /// Colección de productos que pertenecen a esta categoría.
        /// </summary>
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
