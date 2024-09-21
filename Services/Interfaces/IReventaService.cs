using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Services.Interfaces
{
    public interface IReventaService : IGenericService<Reventa> 
    {
        Task<Reventa> PublishTicketAsync(Reventa reventa, int sellerId);
    }
}
