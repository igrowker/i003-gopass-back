using Microsoft.EntityFrameworkCore;
using template_csharp_dotnet.Data;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;

namespace template_csharp_dotnet.Repositories.Classes
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<List<Usuario>> GetAllUsersWithRelations()
        {
            return await _dbSet.Include(x => x.Reventa).ThenInclude(x => x.Entrada).ToListAsync();
        }

        public async Task<Usuario> DeleteUserWithRelations(int id)
        {
            var recordToDelete = await GetById(id);

            if (recordToDelete is null) throw new Exception("El registro no se encontro");

            await _dbSet.Where(x => x.Id == id).Include(x => x.Entrada).ThenInclude(x => x.Reventa).ExecuteDeleteAsync();

            return recordToDelete;
        }
    }
}
