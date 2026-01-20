using Fatura.Services.Interfaces;
using QRCoder;
using System.Drawing;



namespace Fatura.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio para generar códigos QR usando QRCoder.
    /// </summary>
    public class QrService : IQrService
    {
        public byte[] GenerarQr(string data, int size = 200)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new PngByteQRCode(qrCodeData))
                {
                    return qrCode.GetGraphic(20);
                }
            }
        }

        public System.Drawing.Image GenerarQrImage(string data, int size = 200)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new BitmapByteQRCode(qrCodeData))
                {
                    var qrBytes = qrCode.GetGraphic(20);
                    using (var ms = new System.IO.MemoryStream(qrBytes))
                    {
                        return System.Drawing.Image.FromStream(ms);
                    }
                }
            }
        }
    }
}
