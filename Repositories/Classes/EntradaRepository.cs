using template_csharp_dotnet.Data;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;

namespace template_csharp_dotnet.Repositories.Classes
{
    public class EntradaRepository : GenericRepository<Entrada>, IEntradaRepository
    {
        public EntradaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
