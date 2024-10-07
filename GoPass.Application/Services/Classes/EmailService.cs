using GoPass.Domain.DTOs.Request.AuthRequestDTOs;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using GoPass.Application.Services.Interfaces;
using GoPass.Domain.DTOs.Request.Notification;

namespace GoPass.Application.Services.Classes
{
    public class EmailService : IEmailService
    {
        private static string _Host = "smtp.gmail.com";
        private static int _Port = 587;

        private static string _From = "Autenticacion";
        private static string _Email = "automatizaciones.sas@gmail.com";
        private static string _Password = "nnkyigaljcvbydhi";

        public async Task<bool> SendVerificationEmailAsync(EmailValidationRequestDto emailValidationRequestDto)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_From, _Email));
                email.To.Add(MailboxAddress.Parse(emailValidationRequestDto.To));
                email.Subject = emailValidationRequestDto.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = emailValidationRequestDto.Body };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_Host, _Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_Email, _Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                return false;
            }
        }
        //public async Task<bool> SendNotificationEmail(NotificationEmailRequestDto notificationEmailRequestDto)
        public async Task<bool> SendNotificationEmail(string message)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_From, _Email));
                //email.To.Add(MailboxAddress.Parse(notificationEmailRequestDto.To));
                email.To.Add(MailboxAddress.Parse("ezefeola@gmail.com"));
                //email.Subject = notificationEmailRequestDto.Message;
                email.Subject = message;
                 email.Body = new TextPart(TextFormat.Html) { Text = message};

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_Host, _Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_Email, _Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                return false;
            }
        }

    }
}
