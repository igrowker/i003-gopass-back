using Microsoft.EntityFrameworkCore;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Data;
using GoPass.Infrastructure.Repositories.Interfaces;
using GoPass.Domain.DTOs.Request.PaginationDTOs;
using GoPass.Domain.IQueryableExtensions;

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

            await _dbContext.SaveChangesAsync();

            return reventa;
        }

        public async Task<Reventa> GetResaleByEntradaId(int entradaId)
        {
            var resale = await _dbSet.Where(x => x.EntradaId == entradaId).SingleOrDefaultAsync();

            if (resale is null) throw new Exception();

            return resale;
        }

        public override async Task<List<Reventa>> GetAllWithPagination(PaginationDto paginationDto)
        {
            var recordsQueriable = _dbContext.Set<Reventa>().AsQueryable();

            return await recordsQueriable.Paginate(paginationDto).Include(x => x.Entrada).ToListAsync();
        }

        public async Task<List<Reventa>> GetBoughtTicketsByCompradorId(int compradorId)
        {
            if (compradorId <= 0)
            {
                throw new ArgumentException("El ID del vendedor no es válido.");
            }

            List<Reventa> ticketsInResale = await _dbSet.Where(x => x.CompradorId == compradorId).Include(x => x.Entrada).AsNoTracking().ToListAsync();

            return ticketsInResale;
        }
    }
}
