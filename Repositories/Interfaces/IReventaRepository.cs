using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Repositories.Interfaces
{
    public interface IReventaRepository : IGenericRepository<Reventa>
    {
        Task<Reventa> Publish(Reventa reventa, int vendedorId);
        Task<Reventa> GetResaleByEntradaId(int entradaId);
    }
}
