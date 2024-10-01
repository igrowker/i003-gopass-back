using GoPass.Application.DTOs.Request.AuthRequestDTOs;
using GoPass.Application.Services.Classes;
using GoPass.Application.Services.Interfaces;
using GoPass.Application.Utilities.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GoPass.Domain.Models;
using ITemplateService = GoPass.Application.Services.Interfaces.ITemplateService;



namespace GoPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly ITemplateService _templateService;

        public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger, IWebHostEnvironment environment, ITemplateService plantillaServicio, ITemplateService templateService)
        {
            _usuarioService = usuarioService;
            _logger = logger;
            _environment = environment;
            _templateService = templateService; 
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userToRegister = registerRequestDto.FromRegisterToModel();
                userToRegister.Verificado = false;

                var registeredUser = await _usuarioService.RegisterUserAsync(userToRegister);

                if (registeredUser != null)
                {
                    
                    string confirmationUrl = $"{Request.Scheme}://{Request.Host}/Inicio/Confirmar?token={registeredUser.Token}";

                    
                    var valoresReemplazo = new Dictionary<string, string>
                    {
                        { "Nombre", registeredUser.Nombre },
                        { "UrlConfirmacion", confirmationUrl }
                    };

                    
                    string contenidoPlantilla = await _templateService.ObtenerContenidoTemplateAsync("Confirmar", valoresReemplazo);

                   
                    Correo correo = new Correo()
                    {
                        Para = registeredUser.Email,
                        Asunto = "Confirmación de cuenta",
                        Contenido = contenidoPlantilla
                    };

                    
                    bool enviado = await CorreoServicio.EnviarAsync(correo);

                    if (enviado)
                    {
                        return Ok(new
                        {
                            Message = $"Su cuenta ha sido creada. Hemos enviado un mensaje al correo {registeredUser.Email} para confirmar su cuenta."
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error al enviar el correo de confirmación.");
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo crear su cuenta.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el usuario.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al registrar el usuario.");
            }
        }


        [HttpPost("Login")]
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
        [HttpPut("Modify-User-Credentials")]
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
