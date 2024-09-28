﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GoPass.Application.DTOs.Request.ReventaRequestDTOs;
using GoPass.Application.Services.Interfaces;
using GoPass.Application.Utilities.Mappers;

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

            var resaleDb = await _reventaService.GetResaleByEntradaIdAsync(buyEntradaRequestDto.EntradaId);

            if(userId == resaleDb.VendedorId)
            {
                return BadRequest("No podes comprar tu propia entrada flaco que haces estas re loco");
            }

            resaleDb.CompradorId = userId;

            var publishReventaBuyer = await _reventaService.Update(resaleDb.Id, resaleDb);

            return Ok(publishReventaBuyer);
        }
    }
}
