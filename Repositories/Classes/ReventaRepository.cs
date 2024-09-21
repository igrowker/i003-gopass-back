using template_csharp_dotnet.Data;
using template_csharp_dotnet.DTOs.Request.ReventaRequestDTOs;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;

namespace template_csharp_dotnet.Repositories.Classes
{
    public class ReventaRepository : GenericRepository<Reventa>, IReventaRepository
    {
        public ReventaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<Reventa> Publish(Reventa reventa, int vendedorId)
        {
            _dbContext.Add(reventa);
            //reventa.Entrada.UsuarioId = vendedorId; 

            await _dbContext.SaveChangesAsync();

            return reventa;
        }
    }
}
