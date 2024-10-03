using Twilio;
using Twilio.Rest.Verify.V2.Service;
using GoPass.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;


namespace GoPass.Application.Services.Classes
{
    public class TwilioSmsService : ITwilioSmsService
    {
        private readonly IConfiguration _configuration;

        public TwilioSmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendVerificationCode(string toPhoneNumber)
        {
            try
            {
                var accountSid = _configuration["Twilio:AccountSID"];
                var authToken = _configuration["Twilio:AuthToken"];
                var serviceSid = _configuration["Twilio:ServiceSID"];

                TwilioClient.Init(accountSid, authToken);

                var verification = await VerificationResource.CreateAsync(
                    to: toPhoneNumber,
                    channel: "sms",
                    pathServiceSid: serviceSid
                );

                if (verification.Status == "pending")
                {
                    return true; 
                }
                else
                {
                    Console.WriteLine($"Error: Verificación no enviada, estado: {verification.Status}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar código de verificación: {ex.Message}");
                return false;
            }
        }
    }
}
