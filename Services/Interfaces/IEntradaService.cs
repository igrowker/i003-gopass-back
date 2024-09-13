using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Services.Interfaces
{
    public interface IEntradaService 
    {
        Task<List<Entrada>> GetAllTicketsAsync();
        Task<Entrada> GetTicketByIdAsync(int id);
        Task<Entrada> CreateTicketAsync(Entrada entrada);
        Task<Entrada> UpdateTicketAsync(int id, Entrada entrada);
        Task<Entrada> DeleteTicketAsync(int id);
    }
}
