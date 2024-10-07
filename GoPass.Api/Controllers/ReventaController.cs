using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GoPass.Application.Services.Interfaces;
using GoPass.Application.Utilities.Mappers;
using GoPass.Domain.DTOs.Request.ReventaRequestDTOs;
using GoPass.Domain.DTOs.Request.PaginationDTOs;
using GoPass.Domain.Models;
using GoPass.Domain.DTOs.Request.Notification;

namespace GoPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReventaController : ControllerBase
    {
        private readonly IReventaService _reventaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IEntradaService _entradaService;
        private readonly IGopassHttpClientService _gopassHttpClientService;
        private readonly IEmailService _emailService;

        public ReventaController(IReventaService reventaService, IUsuarioService usuarioService, IEntradaService entradaService, IGopassHttpClientService gopassHttpClientService, IEmailService emailService)
        {
            _reventaService = reventaService;
            _usuarioService = usuarioService;
            _entradaService = entradaService;
            _gopassHttpClientService = gopassHttpClientService;
            _emailService = emailService;
        }

        [Authorize]
        [HttpGet("get-resales")]
        public async Task<IActionResult> GetResales([FromQuery] PaginationDto paginationDto)
        {
            List<Reventa> resales = await _reventaService.GetAllWithPaginationAsync(paginationDto);

            return Ok(resales);
        }

        [HttpGet("get-ticket-from-faker")]
        public async Task<IActionResult> GetTicketFromTicketFaker(string codigoQr)
        {
            Entrada verifiedTicket = await _gopassHttpClientService.GetTicketByQrAsync(codigoQr);

            return Ok(verifiedTicket);
        }

        [HttpGet("validate-ticket-from-faker")]
        public async Task<IActionResult> ValidateTicketFromTicketFaker(string codigoQr)
        {
            Entrada verifiedTicket = await _gopassHttpClientService.GetTicketByQrAsync(codigoQr);

            if (verifiedTicket is null) return BadRequest("No se encontro la entrada a validar.");

            verifiedTicket.Verificada = true;

            return Ok(verifiedTicket);
        }

        [Authorize]
        [HttpPost("publicar-entrada-reventa")]
        public async Task<IActionResult> PublishResaleTicket(PublishReventaRequestDto publishReventaRequestDto)
        {
            string authorizationHeader = Request.Headers["Authorization"].ToString();
            string userIdObtainedString = await _usuarioService.GetUserIdByTokenAsync(authorizationHeader);
            int userId = int.Parse(userIdObtainedString);

            bool validUserCredentials = await _usuarioService.ValidateUserCredentialsToPublishTicket(userId);

            if (validUserCredentials == false) return BadRequest("Debe tener todas sus credenciales en regla para poder publicar una entrada");

            Entrada verifiedTicket = await _gopassHttpClientService.GetTicketByQrAsync(publishReventaRequestDto.CodigoQR);
            PublishEntradaRequestDto existingTicketInFaker = verifiedTicket.FromModelToPublishEntradaRequest();

            Entrada createdTicket = await _entradaService.PublishTicket(existingTicketInFaker, userId);

            Reventa reventaToPublish = publishReventaRequestDto.FromPublishReventaRequestToModel();
            reventaToPublish.EntradaId = createdTicket.Id;

            Reventa publishedReventa = await _reventaService.PublishTicketAsync(reventaToPublish, userId);

            return Ok(publishedReventa.FromModelToPublishReventaResponseDto());
        }

        [Authorize]
        [HttpPut("comprar-entrada")]
        public async Task<IActionResult> BuyTicket(BuyEntradaRequestDto buyEntradaRequestDto)
        {
            string authorizationHeader = Request.Headers["Authorization"].ToString();
            string userIdObtainedString = await _usuarioService.GetUserIdByTokenAsync(authorizationHeader);
            int userId = int.Parse(userIdObtainedString);

            Reventa resaleDb = await _reventaService.GetResaleByEntradaIdAsync(buyEntradaRequestDto.EntradaId);

            if (userId == resaleDb.VendedorId)
            {
                return BadRequest("Esta intentando comprar su propia entrada, lo cual no tiene sentido");
            }

            resaleDb.CompradorId = userId;

            Reventa publishReventaBuyer = await _reventaService.Update(resaleDb.Id, resaleDb);

            var updatedResale = publishReventaBuyer;

            // Crear el objeto NotificationEmailRequestDto para el correo
            var notificationEmail = new NotificationEmailRequestDto
            {
                To = "esperanza96_898@birax.org", // Aquí debes obtener el email del usuario comprador
                Message = $"El usuario con ID {userId} ha comprado la entrada con ID {resaleDb.EntradaId}."
            };

            // Notificar la compra vía el observer
            await _emailService.SendNotificationEmailAsync(notificationEmail); // Llama al método correcto

            return Ok(new { message = "Compra realizada y notificación enviada con éxito.", updatedResale });
        }

    }
}
