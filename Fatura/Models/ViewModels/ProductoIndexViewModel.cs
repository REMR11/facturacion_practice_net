using Fatura.Models.Catalogos;
using System.Linq;

namespace Fatura.Models.ViewModels
{
    public class ProductoIndexViewModel
    {
        public IEnumerable<Producto> Productos { get; set; } = Enumerable.Empty<Producto>();
        public Producto NuevoProducto { get; set; } = new();
    }
}
