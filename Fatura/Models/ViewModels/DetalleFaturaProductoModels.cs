using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Models.ViewModels
{
    public class DetalleFaturaProductoModels 
    {
        public int Idfactura { get; set; }
        public int IddetalleFactura { get; set; }
        public int Idproducto { get; set; }
        public int? Codigo { get; set; }
        public string NombreProducto { get; set; } = null!;
        public double? Precio { get; set; }
        public decimal? Total { get; set; }
    }
}

