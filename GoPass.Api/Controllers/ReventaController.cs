using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GoPass.Application.Services.Interfaces;
using GoPass.Application.Utilities.Mappers;
using GoPass.Domain.DTOs.Request.ReventaRequestDTOs;
using GoPass.Domain.DTOs.Request.PaginationDTOs;
using GoPass.Domain.Models;

namespace GoPass.API.Controllers
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
        [HttpGet("get-resales")]
        public async Task<IActionResult> GetResales([FromQuery] PaginationDto paginationDto)
        {
            List<Reventa> resales = await _reventaService.GetReventasAsync(paginationDto);

            return Ok(resales);
        }

        [Authorize]
        [HttpPost("publicar-entrada-reventa")]
        public async Task<IActionResult> PublishResaleTicket(PublishReventaRequestDto publishReventaRequestDto)
        {
            string authorizationHeader = Request.Headers["Authorization"].ToString();
            string userIdObtainedString = await _usuarioService.GetUserIdByTokenAsync(authorizationHeader);
            int userId = int.Parse(userIdObtainedString);

            Reventa reventaToPublish = publishReventaRequestDto.FromPublishReventaRequestToModel();

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

            if(userId == resaleDb.VendedorId)
            {
                return BadRequest("Esta intentando comprar su propia entrada, lo cual no tiene sentido");
            }

            resaleDb.CompradorId = userId;

            Reventa publishReventaBuyer = await _reventaService.Update(resaleDb.Id, resaleDb);

            return Ok(publishReventaBuyer.FromModelToReventaResponseDto());
        }
    }
}
