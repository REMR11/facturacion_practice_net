using Fatura.Models.Facturacion;

namespace Fatura.Services.Interfaces
{
    public interface IFacturaPdfService
    {
        byte[] GenerarPdf(Factura factura);
    }
}
