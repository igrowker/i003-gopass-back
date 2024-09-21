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

        public EntradaController(IEntradaService entradaService)
        {
            _entradaService = entradaService;
        }

        [Authorize]
        [HttpPost("publicar-entrada")]
        public async Task<IActionResult> PublishTicket(EntradaRequestDto entradaRequestDto)
        {
            var ticketToPublish = entradaRequestDto.ToModel();

            var publishedTicket = await _entradaService.Create(ticketToPublish);

            return Ok(publishedTicket);
        }
    }
}
