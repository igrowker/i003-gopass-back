
using GoPass.Domain.DTOs.Request.PaginationDTOs;
using GoPass.Domain.Models;
using System.Threading.Tasks;

namespace GoPass.Application.Services.Interfaces

{
    public interface IReventaService : IGenericService<Reventa> 
    {
        Task<Reventa> PublishTicketAsync(Reventa reventa, int sellerId);
        Task<Reventa> GetResaleByEntradaIdAsync(int entradaId);
        Task<List<Reventa>> GetBoughtTicketsByCompradorIdAsync(int compradorId);
        Task<HistorialCompraVenta> BuyTicketAsync(int reventaId, int compradorId);
    }
}
