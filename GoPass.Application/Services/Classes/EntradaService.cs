using GoPass.Application.Services.Interfaces;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Repositories.Interfaces;

namespace GoPass.Application.Services.Classes
{
    public class EntradaService : GenericService<Entrada>, IEntradaService
    {
        public EntradaService(IEntradaRepository entradaRepository) : base(entradaRepository)
        {
            
        }
    }
}
