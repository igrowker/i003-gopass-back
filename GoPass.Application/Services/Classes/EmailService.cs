using GoPass.Domain.DTOs.Request.AuthRequestDTOs;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace GoPass.Application.Services.Classes
{
    public class EmailService
    {
        private static string _Host = "smtp.gmail.com";
        private static int _Port = 587;

        private static string _From = "Autenticacion";
        private static string _Email = "automatizaciones.sas@gmail.com";
        private static string _Password = "nnkyigaljcvbydhi";

        public static async Task<bool> SendVerificationEmailAsync(EmailValidationRequestDto emailValidationRequestDto)
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
    }
}
