using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;
using template_csharp_dotnet.Services.Interfaces;

namespace template_csharp_dotnet.Services.Classes
{
    public class EntradaService : GenericService<Entrada>, IEntradaService
    {
        public EntradaService(IEntradaRepository entradaRepository) : base(entradaRepository)
        {
            
        }
    }
}
