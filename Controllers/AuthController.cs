using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using template_csharp_dotnet.DTOs.Request.AuthRequestDTOs;
using template_csharp_dotnet.Services.Interfaces;
using template_csharp_dotnet.Utilities.Mappers;

namespace template_csharp_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        //public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {

        //        return BadRequest();
        //    }
        //}

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userToRegister = registerRequestDto.FromRegisterToModel();

                var userInDb = await  _usuarioService.GetUserByEmail(userToRegister.Email); 

                var registeredUser = await _usuarioService.Create(userToRegister);

                return Ok(registeredUser);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var userToLogin = loginRequestDto.FromLoginToModel();

            var logUser = _usuarioService. 
        }

        //private string GenerateJwtToken(ApplicationUser usuario)
        //{
        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, usuario.DNI),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim("usuarioId", usuario.Id.ToString())
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LLAVE"));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: "localhost",
        //        audience: "localhost",
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(30),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
