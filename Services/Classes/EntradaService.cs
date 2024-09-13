using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;
using template_csharp_dotnet.Services.Interfaces;

namespace template_csharp_dotnet.Services.Classes
{
    public class EntradaService : IEntradaService
    {
        private readonly IEntradaRepository _entradaRepository;

        public EntradaService(IEntradaRepository entradaRepository)
        {
            _entradaRepository = entradaRepository;
        }
        public async Task<List<Entrada>> GetAllTicketsAsync()
        {
            var tickets = await _entradaRepository.GetAll();

            return tickets;
        }
        public async Task<Entrada> GetTicketByIdAsync(int id)
        {
            var ticket = await _entradaRepository.GetById(id);

            return ticket;
        }
        public async Task<Entrada> CreateTicketAsync(Entrada entrada)
        {
            var ticketToCreate = await _entradaRepository.Create(entrada);

            return ticketToCreate;
        }
        public async Task<Entrada> UpdateTicketAsync(int id, Entrada entrada)
        {
            var ticketToUpdate = await _entradaRepository.Update(id, entrada);

            return ticketToUpdate;
        }
        public async Task<Entrada> DeleteTicketAsync(int id)
        {
            var ticketToDelete = await _entradaRepository.Delete(id);

            return ticketToDelete;
        }
    }
}
