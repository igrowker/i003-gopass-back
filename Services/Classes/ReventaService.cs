using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;
using template_csharp_dotnet.Services.Interfaces;

namespace template_csharp_dotnet.Services.Classes
{
    public class ReventaService : GenericService<Reventa>, IReventaService
    {
        private readonly IReventaRepository _reventaRepository;

        public ReventaService(IReventaRepository reventaRepository) : base(reventaRepository)
        {
            _reventaRepository = reventaRepository;
        }

        public async Task<Reventa> PublishTicketAsync(Reventa reventa, int sellerId)        
        {
            return await _reventaRepository.Publish(reventa, sellerId);
        }


    }
}
