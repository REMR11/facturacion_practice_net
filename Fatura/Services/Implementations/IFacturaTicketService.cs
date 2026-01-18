using Fatura.Models.Facturacion;

namespace Fatura.Services.Interfaces
{
    public interface IFacturaTicketService
    {
        byte[] GenerarTicket(Factura factura);
    }
}
