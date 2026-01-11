using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Models
{
    public class DetalleFactura
    {
        public int IddetalleFactura { get; set; }
        public int? Idfactura { get; set; }
        public int? Idproducto { get; set; }
        public DateTime? FechaCompra { get; set; }
        public decimal? Total { get; set; }

        public virtual Factura? IdfacturaNavigation { get; set; }
        public virtual Producto? IdproductoNavigation { get; set; }
    }
}
