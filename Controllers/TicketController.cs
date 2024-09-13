using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using template_csharp_dotnet.Data;
using template_csharp_dotnet.DTOs.Request;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost("publicar")]
        public async Task<IActionResult> PublicarEntrada([FromBody] EntradaRequestDto entradaRequestDto)
        {
            //Validacion Entrada
            if (string.IsNullOrEmpty(entradaRequestDto.CodigoQR) || entradaRequestDto.Verificada == false)
            {
                return BadRequest("Entrada Invalida");
            }
            //Publicacion Entrada 

            return Ok("Entrada Publicada!");
        }
        [Authorize]
        [HttpPost("Compra")]
        public IActionResult CompraEntrada([FromBody] Reventa reventa)
        {
            return Ok("Compra de Entrada Exitosa!");
        }
    }
    public class Entrada
    {
       public string CodigoQR { get; set; }
       public bool Verificada { get; set; }
    }

    public class Reventa
    {
        public int EntradaId { get; set; }
        public int VendedorId { get; set; }
        public int CompradorId { get; set; }
    }
}
