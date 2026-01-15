using Fatura.Models.Catalogos;

namespace Fatura.Models.Inventario
{
    public class Inventario
    {
        public int IdInventario { get; set; }

        public int IdProducto { get; set; }
        public Producto Producto { get; set; }


        public Producto Nombre { get; set; }

        public Categoria   Categoria {get; set;}

      

        public int Stock { get; set; }
        public int StockMinimo { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }
}
