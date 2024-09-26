using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using template_csharp_dotnet.Constants;
using template_csharp_dotnet.DTOs.Request.ReventaRequestDTOs;
using template_csharp_dotnet.DTOs.Response;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Services.Interfaces;
using template_csharp_dotnet.Utilities.Exceptions;
using template_csharp_dotnet.Utilities.Mappers;

namespace template_csharp_dotnet.Controllers
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
        public async Task<IActionResult> VerificarEntrada([FromBody] EntradaRequestDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                Entrada ticket = await _ticketmasterService.VerificarEntrada(requestDto.CodigoQR);

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
