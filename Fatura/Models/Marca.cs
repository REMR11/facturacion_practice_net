using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fatura.Models;

namespace Fatura.Models
{
    public partial class Marca
    {
        public Marca()
        {
            Productos = new HashSet<Producto>();
        }

        public int Idmarca { get; set; }
        public string NombreMarca { get; set; } = null!;

        public virtual ICollection<Producto> Productos { get; set; }
    }
}

