
using GoPass.Domain.DTOs.Request.ReventaRequestDTOs;
using GoPass.Domain.Models;

namespace GoPass.Application.Services.Interfaces
{
    public interface IEntradaService : IGenericService<Entrada>
    {
        Task<Entrada> PublishTicket(PublishEntradaRequestDto publishEntradaRequestDto, int userId);
        Task<bool> VerifyQrCodeAsync(string qrCode);
    }
}
