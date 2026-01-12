using ClosedXML.Excel;
using Fatura.Models.Facturacion;
using Fatura.Repositories.Interfaces;
using Fatura.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Fatura.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de reportes.
    /// Contiene la lógica de generación de reportes.
    /// </summary>
    public class ReporteService : IReporteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReporteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MemoryStream> ExportarProductosAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            // Obtener todos los detalles de factura en el rango de fechas
            var todosDetalles = await _unitOfWork.DetalleFacturas.GetAllAsync();
            var detalleFactura = todosDetalles
                .Where(d => d.CreatedAt >= fechaInicio && d.CreatedAt <= fechaFin)
                .ToList();

            // Agrupar por producto y cargar información relacionada
            var ventasProducto = new List<dynamic>();
            var grupos = detalleFactura
                .Where(d => d.IdProducto.HasValue)
                .GroupBy(d => d.IdProducto!.Value);

            foreach (var grupo in grupos)
            {
                var primerDetalle = grupo.First();
                var producto = await _unitOfWork.Productos.GetWithRelationsAsync(grupo.Key);
                
                ventasProducto.Add(new
                {
                    IdProduct = grupo.Key,
                    Cantidad = grupo.Sum(p => p.Cantidad),
                    NombreProducto = producto?.NombreProducto ?? primerDetalle.NombreProducto,
                    Precio = producto?.Precio ?? primerDetalle.PrecioUnitario,
                    Marca = producto?.Marca?.NombreMarca ?? "N/A"
                });
            }

            // Crear DataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Marca", typeof(string));
            dt.Columns.Add("Precio", typeof(double));

            foreach (var item in ventasProducto)
            {
                var producto = item.GetType().GetProperty("IdProduct")?.GetValue(item);
                var cantidad = item.GetType().GetProperty("Cantidad")?.GetValue(item);
                var nombreProducto = item.GetType().GetProperty("NombreProducto")?.GetValue(item)?.ToString() ?? "";
                var marca = item.GetType().GetProperty("Marca")?.GetValue(item)?.ToString() ?? "";
                var precio = item.GetType().GetProperty("Precio")?.GetValue(item)?.ToString() ?? "0";

                dt.Rows.Add(producto, cantidad, nombreProducto, marca, precio);
            }

            dt.TableName = "Datos";

            // Generar Excel
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    return stream;
                }
            }
        }
    }
}
