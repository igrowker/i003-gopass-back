using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using template_csharp_dotnet.DTOs.Request.AuthRequestDTOs;
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
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userToRegister = registerRequestDto.FromRegisterToModel();


                var userInDb = await _usuarioService.GetUserByEmailAsync(userToRegister.Email);


                if (userInDb != null)
                {
                    // Eliminar esta lógica si la verificación de usuario ya existe se maneja en el repositorio o servicio **
                    // return Conflict("El usuario ya está registrado.");
                }

                // Se deberia eliminar esta lógica ya que el hashing de la contraseña debe hacerse en el servicio de usuario y no aquí 
                // Osea en se debe manejar o se esta manejando por asi decirlo UsuarioService.cs
                // Lo que se deberia eliminar es _usuarioService.HasPassword(userToRegister, userToRegister.Password);

                var userCredentials = await _usuarioService.VerifyUserCredentials(userToRegister.DNI, userToRegister.NumeroTelefono);

                if (userCredentials is not null) return BadRequest("Las credenciales ingresadas ya se encuentran registradas:" + userCredentials);

                var registeredUser = await _usuarioService.RegisterUserAsync(userToRegister);


                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                // Registrar la excepción
                _logger.LogError(ex, "Error al registrar el usuario.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al registrar el usuario.");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var userToLogin = loginRequestDto.FromLoginToModel();

            try
            {
                // ** Eliminar esta lógica, ya que AuthenticateAsync ya maneja la autenticación y el manejo de errores de contraseña **
                // var verifiedPassword = _usuarioService.VerifyUserPassword(userToLogin, userToLogin.Password);

                // ** Eliminar la verificación redundante de contraseña **
                // if (verifiedPassword is PasswordVerificationResult.Failed) return Unauthorized("Las credenciales no son validas.");

                var logUser = await _usuarioService.AuthenticateAsync(userToLogin.Email, userToLogin.Password);

                return Ok(logUser.FromModelToLoginResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al autenticar el usuario.");
                return Unauthorized("Las credenciales no son válidas.");
            }
        }
    }
}
