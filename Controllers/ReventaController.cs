using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using template_csharp_dotnet.DTOs.Request.ReventaRequestDTOs;
using template_csharp_dotnet.DTOs.Response.AuthResponseDTOs;
using template_csharp_dotnet.Services.Interfaces;
using template_csharp_dotnet.Utilities.Mappers;

namespace template_csharp_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReventaController : ControllerBase
    {
        private readonly IReventaService _reventaService;
        private readonly IUsuarioService _usuarioService;

        public ReventaController(IReventaService reventaService, IUsuarioService usuarioService)
        {
            _reventaService = reventaService;
            _usuarioService = usuarioService;
        }

        [Authorize]
        [HttpPost("publicar-entrada-reventa")]
        public async Task<IActionResult> PublishResaleTicket(PublishReventaRequestDto publishReventaRequestDto)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();

            var reventaToPublish = publishReventaRequestDto.FromPublishReventaRequestToModel();

            var userId = _usuarioService.GetUserIdByTokenAsync(authorizationHeader);


            //reventaToPublish.Entrada.UsuarioId = userId.Id;

            reventaToPublish.VendedorId = int.Parse(userId);
            //reventaToPublish.EntradaId = entrada.Id;

            var publishedReventa = await _reventaService.PublishTicketAsync(reventaToPublish, reventaToPublish.VendedorId);

            return Ok(publishedReventa);
        }
    }
}
