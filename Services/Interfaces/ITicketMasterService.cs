using template_csharp_dotnet.DTOs.Response;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Services.Interfaces
{
    public interface ITicketMasterService
    {
        Task<Entrada> VerificarEntrada(string ticketId);
    }
}
