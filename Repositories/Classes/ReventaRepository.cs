using Microsoft.EntityFrameworkCore;
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

        public async Task<Reventa> GetResaleByEntradaId(int entradaId)
        {
            var resale = await _dbSet.Where(x => x.EntradaId == entradaId).FirstOrDefaultAsync();

            if (resale is null) throw new Exception();

            return resale;
        }
    }
}
