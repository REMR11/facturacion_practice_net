using Fatura.Models.Enums;
using Fatura.Models.Facturacion;
using Fatura.Repositories.Interfaces;
using Fatura.Services.Interfaces;
using System.Linq;

namespace Fatura.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio del dashboard.
    /// Calcula métricas y proporciona datos agregados para el dashboard.
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> GetIngresosDelMesAsync()
        {
            var ahora = DateTime.UtcNow;
            var inicioMes = new DateTime(ahora.Year, ahora.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddTicks(-1);

            var facturas = await _unitOfWork.Facturas.GetByFechaAsync(inicioMes, finMes);
            return facturas
                .Where(f => f.Estado == EstadoFactura.Pagada)
                .Sum(f => f.Total);
        }

        public async Task<decimal> GetCambioIngresosPorcentajeAsync()
        {
            var ahora = DateTime.UtcNow;
            var inicioMesActual = new DateTime(ahora.Year, ahora.Month, 1);
            var finMesActual = inicioMesActual.AddMonths(1).AddTicks(-1);

            var inicioMesAnterior = inicioMesActual.AddMonths(-1);
            var finMesAnterior = inicioMesActual.AddTicks(-1);

            var facturasMesActual = await _unitOfWork.Facturas.GetByFechaAsync(inicioMesActual, finMesActual);
            var facturasMesAnterior = await _unitOfWork.Facturas.GetByFechaAsync(inicioMesAnterior, finMesAnterior);

            var ingresosMesActual = facturasMesActual
                .Where(f => f.Estado == EstadoFactura.Pagada)
                .Sum(f => f.Total);

            var ingresosMesAnterior = facturasMesAnterior
                .Where(f => f.Estado == EstadoFactura.Pagada)
                .Sum(f => f.Total);

            if (ingresosMesAnterior == 0)
            {
                return ingresosMesActual > 0 ? 100 : 0;
            }

            return ((ingresosMesActual - ingresosMesAnterior) / ingresosMesAnterior) * 100;
        }

        public async Task<int> GetFacturasEmitidasMesAsync()
        {
            var ahora = DateTime.UtcNow;
            var inicioMes = new DateTime(ahora.Year, ahora.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddTicks(-1);

            var facturas = await _unitOfWork.Facturas.GetByFechaAsync(inicioMes, finMes);
            return facturas.Count();
        }

        public async Task<decimal> GetCambioFacturasPorcentajeAsync()
        {
            var ahora = DateTime.UtcNow;
            var inicioMesActual = new DateTime(ahora.Year, ahora.Month, 1);
            var finMesActual = inicioMesActual.AddMonths(1).AddTicks(-1);

            var inicioMesAnterior = inicioMesActual.AddMonths(-1);
            var finMesAnterior = inicioMesActual.AddTicks(-1);

            var facturasMesActual = await _unitOfWork.Facturas.GetByFechaAsync(inicioMesActual, finMesActual);
            var facturasMesAnterior = await _unitOfWork.Facturas.GetByFechaAsync(inicioMesAnterior, finMesAnterior);

            var countMesActual = facturasMesActual.Count();
            var countMesAnterior = facturasMesAnterior.Count();

            if (countMesAnterior == 0)
            {
                return countMesActual > 0 ? 100 : 0;
            }

            return ((countMesActual - countMesAnterior) / (decimal)countMesAnterior) * 100;
        }

        public async Task<int> GetClientesActivosAsync()
        {
            var clientes = await _unitOfWork.Clientes.FindAsync(c => c.Activo);
            return clientes.Count();
        }

        public async Task<decimal> GetCambioClientesPorcentajeAsync()
        {
            var ahora = DateTime.UtcNow;
            var inicioMesActual = new DateTime(ahora.Year, ahora.Month, 1);
            var finMesActual = inicioMesActual.AddMonths(1).AddTicks(-1);

            var inicioMesAnterior = inicioMesActual.AddMonths(-1);
            var finMesAnterior = inicioMesActual.AddTicks(-1);

            var totalActivosActual = await _unitOfWork.Clientes.CountAsync(c =>
                c.Activo && c.CreatedAt <= finMesActual);

            var totalActivosAnterior = await _unitOfWork.Clientes.CountAsync(c =>
                c.Activo && c.CreatedAt <= finMesAnterior);

            if (totalActivosAnterior == 0)
            {
                return totalActivosActual > 0 ? 100 : 0;
            }

            return ((totalActivosActual - totalActivosAnterior) / (decimal)totalActivosAnterior) * 100;
        }

        public async Task<decimal> GetTasaDeCobroAsync()
        {
            var ahora = DateTime.UtcNow;
            var inicioMes = new DateTime(ahora.Year, ahora.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddTicks(-1);

            var facturasMes = await _unitOfWork.Facturas.GetByFechaAsync(inicioMes, finMes);
            var facturasPagadas = facturasMes.Count(f => f.Estado == EstadoFactura.Pagada);
            var totalFacturas = facturasMes.Count(f => f.Estado != EstadoFactura.Borrador);

            if (totalFacturas == 0)
            {
                return 0;
            }

            return facturasPagadas / (decimal)totalFacturas;
        }

        public async Task<decimal> GetCambioTasaCobroPorcentajeAsync()
        {
            var ahora = DateTime.UtcNow;
            var inicioMesActual = new DateTime(ahora.Year, ahora.Month, 1);
            var finMesActual = inicioMesActual.AddMonths(1).AddTicks(-1);

            var inicioMesAnterior = inicioMesActual.AddMonths(-1);
            var finMesAnterior = inicioMesActual.AddTicks(-1);

            var facturasMesActual = await _unitOfWork.Facturas.GetByFechaAsync(inicioMesActual, finMesActual);
            var facturasMesAnterior = await _unitOfWork.Facturas.GetByFechaAsync(inicioMesAnterior, finMesAnterior);

            var pagadasActual = facturasMesActual.Count(f => f.Estado == EstadoFactura.Pagada);
            var totalActual = facturasMesActual.Count(f => f.Estado != EstadoFactura.Borrador);

            var pagadasAnterior = facturasMesAnterior.Count(f => f.Estado == EstadoFactura.Pagada);
            var totalAnterior = facturasMesAnterior.Count(f => f.Estado != EstadoFactura.Borrador);

            var tasaActual = totalActual == 0 ? 0 : pagadasActual / (decimal)totalActual;
            var tasaAnterior = totalAnterior == 0 ? 0 : pagadasAnterior / (decimal)totalAnterior;

            if (tasaAnterior == 0)
            {
                return tasaActual > 0 ? 100 : 0;
            }

            return ((tasaActual - tasaAnterior) / tasaAnterior) * 100;
        }

        public async Task<Dictionary<string, decimal>> GetIngresosMensualesAsync(int meses = 7)
        {
            var resultado = new Dictionary<string, decimal>();
            var ahora = DateTime.UtcNow;

            for (int i = meses - 1; i >= 0; i--)
            {
                var fecha = ahora.AddMonths(-i);
                var inicioMes = new DateTime(fecha.Year, fecha.Month, 1);
                var finMes = inicioMes.AddMonths(1).AddTicks(-1);

                var facturas = await _unitOfWork.Facturas.GetByFechaAsync(inicioMes, finMes);
                var ingresos = facturas
                    .Where(f => f.Estado == EstadoFactura.Pagada)
                    .Sum(f => f.Total);

                var nombreMes = fecha.ToString("MMM", new System.Globalization.CultureInfo("es-ES"));
                resultado[nombreMes] = ingresos;
            }

            return resultado;
        }

        public async Task<IEnumerable<Factura>> GetFacturasRecientesAsync(int cantidad = 5)
        {
            var todasFacturas = await _unitOfWork.Facturas.GetAllAsync();
            return todasFacturas
                .OrderByDescending(f => f.FechaCreacion)
                .Take(cantidad)
                .ToList();
        }
    }
}
