using System.Net.Http.Headers;
using GoPass.Application.Constants;
using GoPass.Application.DTOs.Response;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

[Obsolete("Microservicio TicketMaster a consumir quedo deprecado")]
public class TicketmasterService
{
    const string APILBL = "apikey";
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _url = "https://app.ticketmaster.com/verification/v1/tickets";

    public TicketmasterService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config[$"{Config.APIKEYS}:{Config.TICKETMASTERKEY}"]!;
    }

    public async Task<TicketResponseDto> VerificarEntrada(string ticketId)
    {
        //DN: Quito try-catch ya que debe capturarlo en controller

        var requestUri = BuildRequestUri(ticketId);
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode(); // Lanzará una excepción si la respuesta no es exitosa (4xx, 5xx)

        var ticketDataJson = await response.Content.ReadAsStringAsync();

        // Deserializar la respuesta a un objeto
        var ticketResponse = JsonConvert.DeserializeObject<TicketResponseDto>(ticketDataJson);
        return ticketResponse!;
    }

    private string BuildRequestUri(string ticketId)
    {
        return $"{_url}/{ticketId}?{APILBL}={_apiKey}";
    }
}