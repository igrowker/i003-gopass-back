using Azure.Core;
using GoPass.Application.Services.Interfaces;
using GoPass.Application.Utilities.Mappers;
using GoPass.Domain.DTOs.Request.ReventaRequestDTOs;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Repositories.Interfaces;

namespace GoPass.Application.Services.Classes
{
    public class EntradaService : GenericService<Entrada>, IEntradaService
    {
        public EntradaService(IEntradaRepository entradaRepository) : base(entradaRepository)
        {
            
        }

        public async Task<Entrada> PublishTicket(PublishEntradaRequestDto publishEntradaRequestDto, int userId)
        {

            Entrada entradaExistingFaker = publishEntradaRequestDto.FromPublishEntradaRequestToModel();
            Entrada entradaToCreate = publishEntradaRequestDto.FromEntradaRequestToModel(entradaExistingFaker, userId);

            await _genericRepository.Create(entradaToCreate);

            return entradaToCreate;
        }
    }
}
