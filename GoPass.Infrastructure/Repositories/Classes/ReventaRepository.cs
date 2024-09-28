using Microsoft.EntityFrameworkCore;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Data;
using GoPass.Infrastructure.Repositories.Interfaces;

namespace GoPass.Infrastructure.Repositories.Classes

{
    public class ReventaRepository : GenericRepository<Reventa>, IReventaRepository
    {
        public ReventaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<Reventa> Publish(Reventa reventa, int vendedorId)
        {
            reventa.VendedorId = vendedorId;
            _dbContext.Add(reventa);
            //reventa.Entrada.UsuarioId = vendedorId; 

            await _dbContext.SaveChangesAsync();

            return reventa;
        }

        public async Task<Reventa> GetResaleByEntradaId(int entradaId)
        {
            var resale = await _dbSet.Where(x => x.EntradaId == entradaId).SingleOrDefaultAsync();

            if (resale is null) throw new Exception();

            return resale;
        }
    }
}
