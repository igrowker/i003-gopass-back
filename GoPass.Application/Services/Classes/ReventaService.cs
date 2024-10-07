using Azure.Core;
using GoPass.Application.Notifications.Interfaces;
using GoPass.Application.Services.Interfaces;
using GoPass.Domain.DTOs.Request.ReventaRequestDTOs;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Repositories.Classes;
using GoPass.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace GoPass.Application.Services.Classes
{
    public class ReventaService : GenericService<Reventa>, IReventaService
    {
        private readonly IReventaRepository _reventaRepository;
        private readonly ISubject<string> _subject;

        public ReventaService(IReventaRepository reventaRepository, ISubject<string> subject) : base(reventaRepository)
        {
            _reventaRepository = reventaRepository;
            _subject = subject;
        }

        public async Task<Reventa> PublishTicketAsync(Reventa reventa, int sellerId)        
        {
            return await _reventaRepository.Publish(reventa, sellerId);
        }

        public async Task<Reventa> GetResaleByEntradaIdAsync(int entradaId)
        {
            return await _reventaRepository.GetResaleByEntradaId(entradaId);
        }

        public async Task<List<Reventa>> GetBoughtTicketsByCompradorIdAsync(int compradorId)
        {
            List<Reventa> ticketsInresale = await _reventaRepository.GetBoughtTicketsByCompradorId(compradorId);

            return ticketsInresale;
        }

        public async Task<Reventa> SellBuyNotifications(int id, Reventa reventa)
        {
            Reventa boughtTicket = await _genericRepository.Update(id, reventa);

            await _subject.Notify($"Se ha comprado la entrada: {reventa}");

            return boughtTicket;
        }
    }
}
