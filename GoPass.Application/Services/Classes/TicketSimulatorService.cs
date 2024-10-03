﻿using GoPass.Application.Constants;
using GoPass.Application.Services.Interfaces;
using GoPass.Application.Utilities.Exceptions;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Repositories.Interfaces;

namespace GoPass.Application.Services.Classes
{
    public class TicketSimulatorService : ITicketMasterService
    {
        private readonly IEntradaRepository _entradaRepository;
        public TicketSimulatorService(IEntradaRepository entradaRepository)
        {
            _entradaRepository = entradaRepository;
        }

        public async Task<Entrada> VerificarEntrada(string ticketQr)
        {
            if (string.IsNullOrWhiteSpace(ticketQr)) throw new TicketVerificationException(Messages.ERR_QR_NEEDED);

            Entrada? result = await _entradaRepository.FindAsync(x => x.CodigoQR == ticketQr);

            if (result is null) throw new TicketVerificationException(Messages.ERR_TICKET_UNFOUND);

            if (!result.Verificada) throw new TicketVerificationException(Messages.ERR_TICKET_UNVERIFIED);

            if (string.IsNullOrWhiteSpace(result.CodigoQR)) throw new TicketVerificationException(Messages.ERR_QR_EMPTY);

            return await _entradaRepository.FindAsync(x => x.CodigoQR == ticketQr);
        }
    }
}
