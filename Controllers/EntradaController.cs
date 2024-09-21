using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using template_csharp_dotnet.DTOs.Request.ReventaRequestDTOs;
using template_csharp_dotnet.Services.Classes;
using template_csharp_dotnet.Services.Interfaces;
using template_csharp_dotnet.Utilities.Mappers;

namespace template_csharp_dotnet.Controllers
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
            var userId = _usuarioService.GetUserIdByTokenAsync(authHeader);

            int parsedUserId = int.Parse(userId);

            var ticketToPublish = publishEntradaRequestDto.FromPublishEntradaRequestToModel();

            ticketToPublish.UsuarioId = parsedUserId;

            var publishedTicket = await _entradaService.Create(ticketToPublish);

            return Ok(publishedTicket);
        }
    }
}
