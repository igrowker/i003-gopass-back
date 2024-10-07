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
        [HttpGet("get-tickets")]
        public async Task<IActionResult> GetTickets([FromQuery] PaginationDto paginationDto)
        {
            List<Entrada> tickets = await _entradaService.GetAllWithPaginationAsync(paginationDto);

            return Ok(tickets);
        }
    }
}
