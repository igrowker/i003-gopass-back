using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // Para deserializar respuestas JSON (necesitarás agregar este paquete)
using System.Net.Http.Headers;

public class TicketmasterService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public TicketmasterService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<TicketResponse> VerificarEntrada(string ticketId)
    {
        try
        {
            var requestUri = BuildRequestUri(ticketId);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Lanzará una excepción si la respuesta no es exitosa (4xx, 5xx)

            var ticketDataJson = await response.Content.ReadAsStringAsync();

            // Deserializar la respuesta a un objeto
            var ticketResponse = JsonConvert.DeserializeObject<TicketResponse>(ticketDataJson);
            return ticketResponse;
        }
        catch (HttpRequestException httpRequestException)
        {
            // Loggear el error o manejarlo como se necesite
            Console.WriteLine($"Error en la solicitud HTTP: {httpRequestException.Message}");
            return null; // Puedes devolver un objeto nulo o un mensaje de error personalizado.
        }
        catch (Exception ex)
        {
            // Manejar otras excepciones inesperadas
            Console.WriteLine($"Error inesperado: {ex.Message}");
            return null;
        }
    }

    private string BuildRequestUri(string ticketId)
    {
        return $"https://app.ticketmaster.com/verification/v1/tickets/{ticketId}?apikey={_apiKey}";
    }
}

// Clase para deserializar la respuesta de Ticketmaster
public class TicketResponse
{
    public string TicketId { get; set; }
    public bool IsValid { get; set; }
    public string EventName { get; set; }
    public DateTime EventDate { get; set; }
}