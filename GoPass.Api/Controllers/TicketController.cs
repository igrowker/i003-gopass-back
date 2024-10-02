using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GoPass.Application.Utilities.Exceptions;
using GoPass.Application.Constants;
using GoPass.Domain.Models;
using GoPass.Application.Utilities.Mappers;
using GoPass.Application.Services.Interfaces;
using GoPass.Domain.DTOs.Request.ReventaRequestDTOs;
using GoPass.Domain.DTOs.Response;

namespace GoPass.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketMasterService _ticketmasterService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketMasterService ticketmasterService)
        {
            _ticketmasterService = ticketmasterService;
        }

        [Authorize]
        [HttpPost(Endpoints.TICKET_VERIFY)]
        public async Task<IActionResult> VerificarEntrada([FromBody] VerifyEntradaRequestDto verifyEntradaRequestDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                Entrada ticket = await _ticketmasterService.VerificarEntrada(verifyEntradaRequestDto.CodigoQR);

                EntradaResponseDto entradaResponse = ticket.FromPublishEntradaRequestToResponseDto();

                return Ok(entradaResponse);
            }
            catch (TicketVerificationException ex)
            {
                _logger.LogError(ex.Message);
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, Messages.ERR_TICKET_VERIFY);
                return StatusCode(StatusCodes.Status500InternalServerError, $"{Messages.ERR_TICKET_VERIFY} - {ex.Message}");
            }
        }
    }
}
