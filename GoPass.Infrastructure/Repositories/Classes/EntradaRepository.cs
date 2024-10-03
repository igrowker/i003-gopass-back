using GoPass.Domain.Models;
using GoPass.Infrastructure.Data;
using GoPass.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoPass.Infrastructure.Repositories.Classes
{
    public class EntradaRepository : GenericRepository<Entrada>, IEntradaRepository
    {
        public EntradaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<bool> VerifyQrCodeExists(string qrCode)
        {
            var qrCodeExist = await _dbSet.AnyAsync(u => u.CodigoQR == qrCode);

            return qrCodeExist;
        }
    }
}
