using GoPass.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPass.Application.Services.Interfaces
{
    public interface IGopassHttpClientService
    {
        Task<Entrada> GetTicketByQrAsync(string qrCode);
    }
}
