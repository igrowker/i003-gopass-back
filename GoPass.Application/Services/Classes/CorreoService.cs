using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoPass.Domain.Models;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace GoPass.Application.Services.Classes
{
    public static class CorreoServicio
    {
        private static string _Host = "smtp.gmail.com";
        private static int _Puerto = 587;

        private static string _NombreEnvia = "Autenticacion";
        private static string _Correo = "automatizaciones.sas@gmail.com";
        private static string _Clave = "nnkyigaljcvbydhi";

        public static async Task<bool> EnviarAsync(Correo correo)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_NombreEnvia, _Correo));
                email.To.Add(MailboxAddress.Parse(correo.Para));
                email.Subject = correo.Asunto;
                email.Body = new TextPart(TextFormat.Html) { Text = correo.Contenido };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_Host, _Puerto, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_Correo, _Clave);
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
