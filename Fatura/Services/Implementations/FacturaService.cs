using Fatura.Exceptions;
using Fatura.Models.Facturacion;
using Fatura.Repositories.Interfaces;
using Fatura.Services.Interfaces;

namespace Fatura.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de facturas.
    /// Contiene la lógica de negocio relacionada con facturas.
    /// </summary>
    public class FacturaService : IFacturaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FacturaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Factura>> GetAllAsync()
        {
            return await _unitOfWork.Facturas.GetAllAsync();
        }

        public async Task<Factura?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Facturas.GetByIdAsync(id);
        }

        public async Task<Factura> CreateAsync(Factura factura)
        {
            // Validar que el cliente existe
            if (factura.ClienteId > 0)
            {
                var cliente = await _unitOfWork.Clientes.GetByIdAsync(factura.ClienteId);
                if (cliente == null)
                {
                    throw new EntityNotFoundException("Cliente", factura.ClienteId);
                }
            }

            // Generar número de factura si no existe
            if (string.IsNullOrWhiteSpace(factura.NumeroFactura))
            {
                factura.NumeroFactura = await GenerarNumeroFacturaAsync();
            }

            // Establecer fecha de creación si no está establecida
            if (factura.FechaCreacion == default)
            {
                factura.FechaCreacion = DateTime.UtcNow;
            }

            await _unitOfWork.Facturas.AddAsync(factura);
            await _unitOfWork.SaveChangesAsync();

            return factura;
        }

        public async Task<Factura> UpdateAsync(int id, Factura factura)
        {
            var existingFactura = await _unitOfWork.Facturas.GetByIdAsync(id);
            if (existingFactura == null)
            {
                throw new EntityNotFoundException("Factura", id);
            }

            // Actualizar propiedades
            existingFactura.ClienteId = factura.ClienteId;
            existingFactura.FechaVencimiento = factura.FechaVencimiento;
            existingFactura.Estado = factura.Estado;
            existingFactura.TipoDocumento = factura.TipoDocumento;
            existingFactura.SerieFactura = factura.SerieFactura;
            existingFactura.CaiDte = factura.CaiDte;
            existingFactura.FechaLimiteEmision = factura.FechaLimiteEmision;

            // Recalcular totales si hay cambios en detalles
            await CalcularTotalAsync(id);

            await _unitOfWork.Facturas.UpdateAsync(existingFactura);
            await _unitOfWork.SaveChangesAsync();

            return existingFactura;
        }

        public async Task DeleteAsync(int id)
        {
            var factura = await _unitOfWork.Facturas.GetByIdAsync(id);
            if (factura == null)
            {
                throw new EntityNotFoundException("Factura", id);
            }

            await _unitOfWork.Facturas.DeleteAsync(factura);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<DetalleFactura> AgregarProductoAsync(int facturaId, int productoId, int cantidad = 1)
        {
            // Validar que la factura existe
            var factura = await _unitOfWork.Facturas.GetByIdAsync(facturaId);
            if (factura == null)
            {
                throw new EntityNotFoundException("Factura", facturaId);
            }

            // Validar que el producto existe
            var producto = await _unitOfWork.Productos.GetByIdAsync(productoId);
            if (producto == null)
            {
                throw new EntityNotFoundException("Producto", productoId);
            }

            // Validar stock disponible
            if (producto.Stock < cantidad)
            {
                throw new BusinessRuleException("StockInsuficiente", 
                    $"No hay suficiente stock disponible. Stock actual: {producto.Stock}, solicitado: {cantidad}");
            }

            // Validar que el producto tiene precio
            if (!producto.Precio.HasValue || producto.Precio.Value <= 0)
            {
                throw new BusinessRuleException("PrecioInvalido", 
                    "El producto no tiene un precio válido asignado.");
            }

            // Crear detalle de factura
            var detalleFactura = new DetalleFactura
            {
                IdFactura = facturaId,
                IdProducto = productoId,
                NombreProducto = producto.NombreProducto,
                UnidadMedida = producto.UnidadMedida?.Abreviatura,
                Cantidad = cantidad,
                PrecioUnitario = producto.Precio.Value,
                Descuento = 0
            };

            await _unitOfWork.DetalleFacturas.AddAsync(detalleFactura);

            // Actualizar stock del producto
            producto.Stock -= cantidad;
            await _unitOfWork.Productos.UpdateAsync(producto);

            // Recalcular totales de la factura
            await CalcularTotalAsync(facturaId);

            await _unitOfWork.SaveChangesAsync();

            return detalleFactura;
        }

        public async Task CalcularTotalAsync(int facturaId)
        {
            var factura = await _unitOfWork.Facturas.GetWithDetailsAsync(facturaId);
            if (factura == null)
            {
                throw new EntityNotFoundException("Factura", facturaId);
            }

            decimal subTotal = 0;
            decimal impuesto = 0;

            foreach (var detalle in factura.DetalleFacturas)
            {
                var totalDetalle = (detalle.Cantidad * detalle.PrecioUnitario) - detalle.Descuento;
                subTotal += totalDetalle;

                // Calcular impuestos del detalle (si hay impuestos configurados)
                // Por ahora, asumimos que los impuestos se calculan después
            }

            // Calcular impuestos (IVA, ISR, etc.)
            // Por simplicidad, asumimos un IVA del 13%
            decimal iva = subTotal * 0.13m;
            impuesto = iva;

            factura.SubTotal = subTotal;
            factura.Iva = iva;
            factura.Isr = 0; // Se puede calcular según reglas de negocio
            factura.OtrosImpuestos = 0;
            factura.Total = subTotal + impuesto;

            await _unitOfWork.Facturas.UpdateAsync(factura);
        }

        public async Task<Factura> GetWithDetailsAsync(int id)
        {
            var factura = await _unitOfWork.Facturas.GetWithDetailsAndProductsAsync(id);
            if (factura == null)
            {
                throw new EntityNotFoundException("Factura", id);
            }
            return factura;
        }

        private async Task<string> GenerarNumeroFacturaAsync()
        {
            var year = DateTime.UtcNow.Year;
            var count = await _unitOfWork.Facturas.CountAsync();
            var numero = (count + 1).ToString("D4");
            return $"FAC-{year}-{numero}";
        }
    }
}
