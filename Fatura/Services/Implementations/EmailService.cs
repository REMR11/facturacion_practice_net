using System.Text;
using Fatura.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Fatura.Services.Implementations
{
    /// <summary>
    /// Servicio para el envío de correos electrónicos usando MailKit (compatible con Gmail).
    /// Gmail requiere: verificación en 2 pasos activada y contraseña de aplicación (no la contraseña normal).
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

        /// <inheritdoc />
        public async Task<bool> EnviarCorreoConAdjuntoAsync(string destinatario, string asunto, string cuerpo, byte[] archivoAdjunto, string nombreArchivo)
        {
            var smtpHost = _configuration["Email:SmtpHost"];
            var smtpPort = _configuration.GetValue<int>("Email:SmtpPort", 587);
            var smtpUsuario = _configuration["Email:SmtpUsuario"];
            var smtpPassword = _configuration["Email:SmtpPassword"];
            var smtpFrom = _configuration["Email:SmtpFrom"];
            var smtpFromName = _configuration["Email:SmtpFromName"] ?? "Sistema de Facturación";

            if (string.IsNullOrWhiteSpace(smtpHost) ||
                string.IsNullOrWhiteSpace(smtpUsuario) ||
                string.IsNullOrWhiteSpace(smtpPassword) ||
                string.IsNullOrWhiteSpace(smtpFrom))
            {
                _logger.LogWarning("Configuración de correo incompleta. Configure Email:SmtpHost, SmtpUsuario, SmtpPassword y SmtpFrom en appsettings o User Secrets.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(destinatario) || !destinatario.Contains("@"))
            {
                _logger.LogWarning("Dirección de correo inválida: {Destinatario}", destinatario);
                return false;
            }

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(smtpFromName, smtpFrom));
                message.To.Add(MailboxAddress.Parse(destinatario));
                message.Subject = asunto;

                var builder = new BodyBuilder
                {
                    HtmlBody = cuerpo
                };

                if (archivoAdjunto != null && archivoAdjunto.Length > 0)
                {
                    builder.Attachments.Add(nombreArchivo, archivoAdjunto, new ContentType("application", "pdf"));
                }

                message.Body = builder.ToMessageBody();

                using var client = new SmtpClient();
                // Gmail: 587 = STARTTLS, 465 = SSL implícito
                var secureSocketOptions = smtpPort == 465
                    ? SecureSocketOptions.SslOnConnect
                    : SecureSocketOptions.StartTls;
                await client.ConnectAsync(smtpHost, smtpPort, secureSocketOptions);
                await client.AuthenticateAsync(smtpUsuario, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Correo enviado correctamente a {Destinatario}", destinatario);
                return true;
            }
            catch (AuthenticationException ex)
            {
                _logger.LogError(ex, "Error de autenticación SMTP (Gmail: use contraseña de aplicación, no la contraseña normal). Ver https://support.google.com/mail/answer/185833");
                return false;
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "";
                _logger.LogError(ex, "Error al enviar correo a {Destinatario}: {Message}. {Inner}", destinatario, ex.Message, inner);
                return false;
            }
        }
    }
}
