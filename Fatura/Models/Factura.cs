using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Models
{
    public partial  class Factura 
    {
        public Factura()
        {
            DetalleFacturas = new HashSet<DetalleFactura>();
        }

        public int Idfactura { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public decimal? Total { get; set; }

        public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; }
    }
}
