using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userToRegister = registerRequestDto.FromRegisterToModel();

                var userInDb = await  _usuarioService.GetUserByEmailAsync(userToRegister.Email); 

                _usuarioService.HasPassword(userToRegister, userToRegister.Password);

                var registeredUser = await _usuarioService.RegisterUserAsync(userToRegister);

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


            var logUser = await _usuarioService.AuthenticateAsync(userToLogin.Email, userToLogin.Password);
            var verifiedPassword = _usuarioService.VerifyUserPassword(userToLogin, userToLogin.Password);

            if (verifiedPassword is PasswordVerificationResult.Failed) return Unauthorized("Las credenciales no son validas.");

            return Ok(logUser);
        }

        
    }
}
