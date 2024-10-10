using GoPass.Domain.Models;
using GoPass.Infrastructure.Data;
using GoPass.Infrastructure.Repositories.Interfaces;

namespace GoPass.Infrastructure.Repositories.Classes
{
    public class HistorialCompraVentaRepository : GenericRepository<HistorialCompraVenta>, IHistorialCompraVentaRepository
    {
        public HistorialCompraVentaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
                
        }
    }
}
