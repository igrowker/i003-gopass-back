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
        private readonly IEntradaService _entradaService;

        public ReventaController(IReventaService reventaService, IUsuarioService usuarioService, IEntradaService entradaService)
        {
            _reventaService = reventaService;
            _usuarioService = usuarioService;
            _entradaService = entradaService;
        }

        [Authorize]
        [HttpPost("publicar-entrada-reventa")]
        public async Task<IActionResult> PublishResaleTicket(PublishReventaRequestDto publishReventaRequestDto)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var userIdObtainedString = await _usuarioService.GetUserIdByTokenAsync(authorizationHeader);
            var userId = int.Parse(userIdObtainedString);

            var reventaToPublish = publishReventaRequestDto.FromPublishReventaRequestToModel();

            var publishedReventa = await _reventaService.PublishTicketAsync(reventaToPublish, userId);

            return Ok(publishedReventa);
        }

        [Authorize]
        [HttpPut("comprar-entrada")]
        public async Task<IActionResult> BuyTicket(BuyEntradaRequestDto buyEntradaRequestDto)
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var userIdObtainedString = await _usuarioService.GetUserIdByTokenAsync(authorizationHeader);
            var userId = int.Parse(userIdObtainedString);

            var publishedReventa = buyEntradaRequestDto.FromBuyEntradaRequestToModel();

            var resaleDb = await _reventaService.GetResaleByEntradaIdAsync(publishedReventa.EntradaId);

            if(userId == resaleDb.VendedorId)
            {
                return BadRequest("No podes comprar tu propia entrada flaco que haces estas re loco");
            }

            publishedReventa.VendedorId = resaleDb.VendedorId;
            publishedReventa.Precio = resaleDb.Precio;
            publishedReventa.FechaReventa = resaleDb.FechaReventa;
            publishedReventa.CompradorId = userId;

            var publishReventaBuyer = await _reventaService.Update(resaleDb.Id, publishedReventa);

            return Ok(publishReventaBuyer);
        }
    }
}
