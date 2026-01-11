using Fatura.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

namespace Fatura.Controllers
{
    public class ReporteController : Controller
    {
        private readonly xstoreContext _context;

        public ReporteController(xstoreContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<FileResult> ExportarProductos(DateTime fechainicio, DateTime fechafin)
        {


            var detalleFactura = await _context.DetalleFacturas.Include((d) => d.Producto)
                                                     .ThenInclude((p) => p.Marca)
                                                     //.Where((c) => c.CreatedAt >= fechainicio && c.CreatedAt <= fechafin)
                                                     .ToListAsync();

            var ventasProducto = detalleFactura.GroupBy((d) => d.IdProducto)
                                        .Select((g) =>
                                            new {
                                                IdProduct = g.Key,
                                                Cantidad = (int)g.Sum(p => p.Cantidad),
                                                NombreProducto = g.First()?.Producto?.NombreProducto,
                                                Precio = g.First()?.Producto?.Precio,
                                                Marca = g.First()?.Producto?.Marca?.NombreMarca
                                            }).ToList();


            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Producto", typeof(String));
            dt.Columns.Add("Marca", typeof(String));
            dt.Columns.Add("Precio", typeof(double));

            foreach (var producto in ventasProducto)
            {
                dt.Rows.Add(producto.IdProduct, producto.Cantidad, producto.NombreProducto, producto.Marca, producto.Precio);
            }



            dt.TableName = "Datos";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte Recepciones " + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }
    }
}