using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Models
{
    public partial  class Categorium 
    {
        public Categorium()
        {
            Productos = new HashSet<Producto>();
        }

        public int Idcategoria { get; set; }
        public string NombreCategoria { get; set; } = null!;

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
