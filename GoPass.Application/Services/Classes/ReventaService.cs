using GoPass.Application.Services.Interfaces;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Repositories.Interfaces;

namespace GoPass.Application.Services.Classes
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

        public async Task<Reventa> GetResaleByEntradaIdAsync(int entradaId)
        {
            return await _reventaRepository.GetResaleByEntradaId(entradaId);
        }

    }
}
