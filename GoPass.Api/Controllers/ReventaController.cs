﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GoPass.Application.Services.Interfaces;
using GoPass.Application.Utilities.Mappers;
using GoPass.Domain.DTOs.Request.ReventaRequestDTOs;
using GoPass.Domain.DTOs.Request.PaginationDTOs;
using GoPass.Domain.Models;
using GoPass.Domain.DTOs.Response.AuthResponseDTOs;

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

        public ReventaController(IReventaService reventaService, IUsuarioService usuarioService, IEntradaService entradaService, IGopassHttpClientService gopassHttpClientService)
        {
            _reventaService = reventaService;
            _usuarioService = usuarioService;
            _entradaService = entradaService;
            _gopassHttpClientService = gopassHttpClientService;
        }

        [Authorize]
        [HttpGet("get-resales")]
        public async Task<IActionResult> GetResales([FromQuery] PaginationDto paginationDto)
        {
            List<Reventa> resales = await _reventaService.GetAllWithPaginationAsync(paginationDto);

            return Ok(resales);
        }

        [Authorize]
        [HttpGet("get-seller-information")]
        public async Task<IActionResult> GetTicketResaleSellerInformation(int vendedorId)
        {
            Usuario sellerInformation = await _usuarioService.GetByIdAsync(vendedorId);

            SellerInformationResponseDto sellerInformationResponseDto = sellerInformation.FromModelToSellerInformationResponseDto();

            return Ok(sellerInformationResponseDto);
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
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

            HistorialCompraVenta publishReventaBuyer = await _reventaService.BuyTicketAsync(resaleDb.Id, userId);

            return Ok(publishReventaBuyer);
        }
    }
}
