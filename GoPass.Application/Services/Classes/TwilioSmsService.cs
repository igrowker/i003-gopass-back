using System;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
using System.Threading.Tasks;
using GoPass.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Twilio.Types;
using Twilio.TwiML.Messaging;


namespace GoPass.Application.Services.Classes
{
    public class TwilioSmsService : ITwilioSmsService
    {
        private readonly IConfiguration _configuration;

        public TwilioSmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public async Task SendVerificationCode(string toPhoneNumber)
        //{

        //    var accountSid = _configuration["Twilio:AccountSID"];
        //    var authToken = _configuration["Twilio:AuthToken"];
        //    //var fromPhoneNumber = _configuration["Twilio:FromPhoneNumber"];
        //    var serviceSid = _configuration["Twilio:ServiceSID"];

        //    TwilioClient.Init(accountSid, authToken);

        //    var verification = await VerificationResource.CreateAsync(
        //    to: $"{toPhoneNumber}",
        //    channel: "sms",
        //    pathServiceSid: serviceSid);
        //}

        public async Task<bool> SendVerificationCode(string toPhoneNumber)
        {
            try
            {
                //var accountSid = _configuration["Twilio:AccountSID"];
                //var authToken = _configuration["Twilio:AuthToken"];
                //var serviceSid = _configuration["Twilio:ServiceSID"];
                var accountSid = "Para el commit necesitas que el _configuration este comentado";
                var authToken = "Para el commit necesitas que el _configuration este comentado";
                var serviceSid = "Para el commit necesitas que el _configuration este comentado";

                TwilioClient.Init(accountSid, authToken);

                // Envía el código de verificación vía SMS
                var verification = await VerificationResource.CreateAsync(
                    to: toPhoneNumber,
                    channel: "sms",
                    pathServiceSid: serviceSid
                );

                // Verifica si el estado es "pending", lo que indica que el SMS fue enviado
                if (verification.Status == "pending")
                {
                    return true; // El SMS fue enviado correctamente
                }
                else
                {
                    // Aquí puedes manejar otros estados que podría devolver la API
                    Console.WriteLine($"Error: Verificación no enviada, estado: {verification.Status}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores, podría loguear el error o lanzar una excepción personalizada
                Console.WriteLine($"Error al enviar código de verificación: {ex.Message}");
                return false;
            }
        }
    }
}
