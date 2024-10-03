﻿
using GoPass.Domain.DTOs.Request.PaginationDTOs;
using GoPass.Domain.Models;

namespace GoPass.Application.Services.Interfaces

{
    public interface IReventaService : IGenericService<Reventa> 
    {
        Task<Reventa> PublishTicketAsync(Reventa reventa, int sellerId);
        Task<Reventa> GetResaleByEntradaIdAsync(int entradaId);
    }
}
