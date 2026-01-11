using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fatura.Models;

namespace Fatura.Models
{
    public  partial class Producto
    {
        public Producto()
        {
            DetalleFacturas = new HashSet<DetalleFactura>();
        }

        public int Idproducto { get; set; }
        public int? Idmarca { get; set; }
        public int? Idcategoria { get; set; }
        public string NombreProducto { get; set; } = null!;
        public double? Precio { get; set; }
        public int? Codigo { get; set; }

        public virtual Categorium? IdcategoriaNavigation { get; set; }
        public virtual Marca? IdmarcaNavigation { get; set; }
        public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; }
    }
}

