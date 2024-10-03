﻿
using GoPass.Domain.Models;

namespace GoPass.Infrastructure.Repositories.Interfaces
{
    public interface IEntradaRepository : IGenericRepository<Entrada>
    {
        Task<bool> VerifyQrCodeExists(string qrCode);
    }
}
