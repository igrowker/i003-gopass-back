using GoPass.Domain.Models;
using GoPass.Infrastructure.Data;
using GoPass.Infrastructure.Repositories.Interfaces;

namespace GoPass.Infrastructure.Repositories.Classes
{
    public class EntradaRepository : GenericRepository<Entrada>, IEntradaRepository
    {
        public EntradaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
