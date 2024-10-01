using GoPass.Application.Services.Interfaces;
using GoPass.Application.Utilities.Mappers;
using GoPass.Domain.DTOs.Request.AuthRequestDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userToRegister = registerRequestDto.FromRegisterToModel();

                userToRegister.Verificado = false;
                var registeredUser = await _usuarioService.RegisterUserAsync(userToRegister);

                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el usuario.");
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userToLogin = loginRequestDto.FromLoginToModel();

                var logUser = await _usuarioService.AuthenticateAsync(userToLogin.Email, userToLogin.Password);

                return Ok(logUser.FromModelToLoginResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al autenticar el usuario.");
                return Unauthorized("Las credenciales no son válidas.");
            }
        }

        [Authorize]
        [HttpGet("user-credentials")]
        public async Task<IActionResult> GetUserCredentials()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var userIdObtainedString = await _usuarioService.GetUserIdByTokenAsync(authHeader);
            int userId = int.Parse(userIdObtainedString);
            var dbExistingUserCredentials = await _usuarioService.GetByIdAsync(userId);

            return Ok(dbExistingUserCredentials);
        }

        [Authorize]
        [HttpPut("modify-user-credentials")]
        public async Task<IActionResult> ModifyUserCredentials(ModifyUsuarioRequestDto modifyUsuarioRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                var userIdObtainedString = await _usuarioService.GetUserIdByTokenAsync(authHeader);
                int userId = int.Parse(userIdObtainedString);
                var dbExistingUserCredentials = await _usuarioService.GetByIdAsync(userId);


                var credentialsToModify = modifyUsuarioRequestDto.FromModifyUsuarioRequestToModel(dbExistingUserCredentials);


                var modifiedCredentials = await _usuarioService.Update(userId, credentialsToModify);

                return Ok(modifiedCredentials);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
