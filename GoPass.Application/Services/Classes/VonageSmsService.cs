using GoPass.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace GoPass.Application.Services.Classes
{
    public class VonageSmsService : IVonageSmsService
    {
        private readonly IConfiguration _configuration;
        private int _verificationCode;
        public VonageSmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendVonageVerificationCode(string phoneNumber)
        {
            var apiKey = _configuration["Vonage:VonageApiKey"];
            var apiSecret = _configuration["Vonage:ApiSecret"];
            // Credenciales de tu cuenta Vonage (API Key y Secret)
            var credentials = Credentials.FromApiKeyAndSecret(apiKey, apiSecret);

            // Crear instancia del cliente de Vonage
            var client = new VonageClient(credentials);

            // Generar un código de verificación aleatorio
            _verificationCode = new Random().Next(100000, 999999);

            // Enviar SMS con el código de verificación
            //var response = await client.SmsClient.SendAnSmsAsync(new SendSmsRequest
            //{
            //    To = phoneNumber, // Número de teléfono del usuario en formato internacional
            //    From = "GopassTest",      // Nombre o número de remitente
            //    Text = $"Tu código de verificación es {codigoVerificacion}" // Mensaje con el código
            //});

            try
            {
                // Enviar SMS con el código de verificación
                var response = await client.SmsClient.SendAnSmsAsync(new SendSmsRequest
                {
                    To = phoneNumber, // Número de teléfono del usuario en formato internacional
                    From = "GopassTest", // Nombre o número de remitente
                    Text = $"Tu código de verificación es {_verificationCode}" // Mensaje con el código
                });

                var message = response.Messages.FirstOrDefault(); // Asegúrate de que estás accediendo a la colección

                // Verificar que la colección no sea nula y tenga al menos un mensaje
                if (message != null && message.Status == "0")
                {
                    Console.WriteLine("SMS enviado con éxito 🚀");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error: {message?.ErrorText ?? "No se pudo enviar el SMS"}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar código: {ex.Message}");
                return false;

            }
        }

        public bool VerifyCode(int userInputCode)
        {
            if (userInputCode == _verificationCode)
            {
                Console.WriteLine("Código verificado con éxito ✅");
                return true;
            }
            else
            {
                Console.WriteLine("Código incorrecto ❌");
                return false;
            }
        }
    }
}
