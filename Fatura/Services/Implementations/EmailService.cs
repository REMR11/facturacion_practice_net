using System.Net;
using System.Net.Mail;
using System.Text;
using Fatura.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Fatura.Services.Implementations
{
    /// <summary>
    /// Servicio para el envío de correos electrónicos usando SMTP.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Envía un correo electrónico con un archivo PDF adjunto.
        /// </summary>
        public async Task<bool> EnviarCorreoConAdjuntoAsync(string destinatario, string asunto, string cuerpo, byte[] archivoAdjunto, string nombreArchivo)
        {
            try
            {
                // Obtener configuración SMTP
                var smtpHost = _configuration["Email:SmtpHost"];
                var smtpPort = _configuration.GetValue<int>("Email:SmtpPort", 587);
                var smtpUsuario = _configuration["Email:SmtpUsuario"];
                var smtpPassword = _configuration["Email:SmtpPassword"];
                var smtpFrom = _configuration["Email:SmtpFrom"];
                var smtpFromName = _configuration["Email:SmtpFromName"] ?? "Sistema de Facturación";
                var enableSsl = _configuration.GetValue<bool>("Email:EnableSsl", true);

                // Validar configuración
                if (string.IsNullOrWhiteSpace(smtpHost) || 
                    string.IsNullOrWhiteSpace(smtpUsuario) || 
                    string.IsNullOrWhiteSpace(smtpPassword) ||
                    string.IsNullOrWhiteSpace(smtpFrom))
                {
                    _logger.LogWarning("Configuración de correo incompleta. No se puede enviar el correo.");
                    return false;
                }

                // Validar destinatario
                if (string.IsNullOrWhiteSpace(destinatario) || !destinatario.Contains("@"))
                {
                    _logger.LogWarning($"Dirección de correo inválida: {destinatario}");
                    return false;
                }

                // Crear mensaje
                using var mensaje = new MailMessage();
                mensaje.From = new MailAddress(smtpFrom, smtpFromName, Encoding.UTF8);
                mensaje.To.Add(new MailAddress(destinatario));
                mensaje.Subject = asunto;
                mensaje.SubjectEncoding = Encoding.UTF8;
                mensaje.Body = cuerpo;
                mensaje.BodyEncoding = Encoding.UTF8;
                mensaje.IsBodyHtml = true;

                // Adjuntar PDF
                if (archivoAdjunto != null && archivoAdjunto.Length > 0)
                {
                    using var stream = new MemoryStream(archivoAdjunto);
                    var adjunto = new Attachment(stream, nombreArchivo, "application/pdf");
                    mensaje.Attachments.Add(adjunto);
                }

                // Configurar cliente SMTP
                using var cliente = new SmtpClient(smtpHost, smtpPort);
                cliente.Credentials = new NetworkCredential(smtpUsuario, smtpPassword);
                cliente.EnableSsl = enableSsl;
                cliente.Timeout = 30000; // 30 segundos

                // Enviar correo
                await cliente.SendMailAsync(mensaje);
                
                _logger.LogInformation($"Correo enviado exitosamente a {destinatario}");
                return true;
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, $"Error SMTP al enviar correo a {destinatario}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al enviar correo a {destinatario}: {ex.Message}");
                return false;
            }
        }
    }
}
