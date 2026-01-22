namespace Fatura.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de envío de correos electrónicos.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Envía un correo electrónico con un archivo PDF adjunto.
        /// </summary>
        /// <param name="destinatario">Dirección de correo del destinatario.</param>
        /// <param name="asunto">Asunto del correo.</param>
        /// <param name="cuerpo">Cuerpo del mensaje (puede ser HTML).</param>
        /// <param name="archivoAdjunto">Bytes del archivo PDF a adjuntar.</param>
        /// <param name="nombreArchivo">Nombre del archivo adjunto.</param>
        /// <returns>True si el correo se envió exitosamente, false en caso contrario.</returns>
        Task<bool> EnviarCorreoConAdjuntoAsync(string destinatario, string asunto, string cuerpo, byte[] archivoAdjunto, string nombreArchivo);
    }
}
