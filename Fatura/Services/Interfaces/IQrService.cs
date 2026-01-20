namespace Fatura.Services.Interfaces
{
    /// <summary>
    /// Servicio para generar códigos QR.
    /// </summary>
    public interface IQrService
    {
        /// <summary>
        /// Genera un código QR como array de bytes (imagen PNG).
        /// </summary>
        /// <param name="data">Datos a codificar en el QR</param>
        /// <param name="size">Tamaño del QR en píxeles (por defecto 200)</param>
        /// <returns>Array de bytes de la imagen PNG del QR</returns>
        byte[] GenerarQr(string data, int size = 200);

        /// <summary>
        /// Genera un código QR como imagen System.Drawing.Image.
        /// </summary>
        /// <param name="data">Datos a codificar en el QR</param>
        /// <param name="size">Tamaño del QR en píxeles (por defecto 200)</param>
        /// <returns>Imagen del QR</returns>
        System.Drawing.Image GenerarQrImage(string data, int size = 200);
    }
}
