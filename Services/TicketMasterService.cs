namespace template_csharp_dotnet.Services
{
    public class TicketMasterService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TicketMasterService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<TicketResponse> VerificarEntrada()
        {

        }
    }
}
