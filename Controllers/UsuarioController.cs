using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using template_csharp_dotnet.DTOs.Request;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Services.Interfaces;
using template_csharp_dotnet.Utilities.Mappers;

namespace template_csharp_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("Get-Users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _usuarioService.GetAllUsers();

                return Ok(users);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPost("Create-User")]
        public async Task<IActionResult> CreateUser(UsuarioRequestDto usuarioRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userToCreate = usuarioRequestDto.ToModel();
                await _usuarioService.CreateUser(userToCreate);

                return Ok(userToCreate.ToResponseDto());
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

    }
}
