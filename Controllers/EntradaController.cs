using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GoPass.Application.Services.Interfaces;
using GoPass.Application.DTOs.Request.ReventaRequestDTOs;
using GoPass.Application.Utilities.Mappers;

namespace GoPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradaController : ControllerBase
    {
        private readonly IEntradaService _entradaService;
        private readonly IUsuarioService _usuarioService;

        public EntradaController(IEntradaService entradaService, IUsuarioService usuarioService)
        {
            _entradaService = entradaService;
            _usuarioService = usuarioService;
        }

        [Authorize]
        [HttpPost("publicar-entrada")]
        public async Task<IActionResult> PublishTicket(PublishEntradaRequestDto publishEntradaRequestDto)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var userId = await _usuarioService.GetUserIdByTokenAsync(authHeader);
            int parsedUserId = int.Parse(userId);

            var ticketToPublish = publishEntradaRequestDto.FromPublishEntradaRequestToModel();

            ticketToPublish.UsuarioId = parsedUserId;

            var publishedTicket = await _entradaService.Create(ticketToPublish);

            return Ok(publishedTicket.FromPublishEntradaRequestToResponseDto());
        }
    }
}
