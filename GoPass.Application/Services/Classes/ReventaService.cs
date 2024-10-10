using Azure.Core;
using GoPass.Application.Services.Interfaces;
using GoPass.Domain.DTOs.Request.ReventaRequestDTOs;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Repositories.Classes;
using GoPass.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoPass.Application.Services.Classes
{
    public class ReventaService : GenericService<Reventa>, IReventaService
    {
        private readonly IReventaRepository _reventaRepository;
        private readonly IEntradaRepository _entradaRepository;
        private readonly IHistorialCompraVentaRepository _historialCompraVentaRepository;

        public ReventaService(IReventaRepository reventaRepository, IEntradaRepository entradaRepository, 
            IHistorialCompraVentaRepository historialCompraVentaRepository) : base(reventaRepository)
        {
            _reventaRepository = reventaRepository;
            _entradaRepository = entradaRepository;
            _historialCompraVentaRepository = historialCompraVentaRepository;
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

        public async Task<HistorialCompraVenta> BuyTicketAsync(int reventaId, int compradorId)
        {
            Reventa resale = await _reventaRepository.GetById(reventaId);

            Entrada ticket = await _entradaRepository.GetById(resale.EntradaId);

            HistorialCompraVenta historialCompra = new HistorialCompraVenta
            {
                GameName = ticket.GameName,
                Description = ticket.Description,
                Image = ticket.Image,
                Address = ticket.Address,
                EventDate = ticket.EventDate,
                CodigoQR = ticket.CodigoQR,
                Verificada = ticket.Verificada,
                EntradaId = ticket.Id,
                VendedorId = resale.VendedorId,
                CompradorId = compradorId,
                FechaReventa = DateTime.Now,
                Precio = resale.Precio,
                ResaleDetail = resale.ResaleDetail
            };

            await _historialCompraVentaRepository.Create(historialCompra);

            await _reventaRepository.Delete(reventaId);
            await _entradaRepository.Delete(resale.EntradaId);

            return historialCompra;
        }
    }
}
