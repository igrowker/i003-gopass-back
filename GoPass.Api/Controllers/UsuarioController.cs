using GoPass.Application.DTOs.Request.AuthRequestDTOs;
using GoPass.Application.Services.Classes;
using GoPass.Application.Services.Interfaces;
using GoPass.Application.Utilities.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GoPass.Domain.Models;
using ITemplateService = GoPass.Application.Services.Interfaces.ITemplateService;
using Microsoft.AspNetCore.Identity;



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
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger, IWebHostEnvironment environment, ITemplateService plantillaServicio, ITemplateService templateService, ITokenService tokenService, IPasswordHasher<Usuario> passwordHasher)
        {
            _usuarioService = usuarioService;
            _logger = logger;
            _environment = environment;
            _templateService = templateService;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
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
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Error al registrar el usuario: " + argEx.Message);
                return BadRequest(argEx.Message);
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userToLogin = loginRequestDto.FromLoginToModel();


                var usuario = await _usuarioService.AuthenticateAsync(userToLogin.Email, userToLogin.Password);

                if (usuario == null)
                {
                    return Unauthorized(new { mensaje = "Credenciales incorrectas." });
                }


                if (!usuario.Verificado)
                {
                    return Ok(new { mensaje = $"Falta confirmar su cuenta. Se le envió un correo a {userToLogin.Email}" });
                }
                var token = _tokenService.CreateToken(usuario); 


                await _usuarioService.UpdateUserTokenAsync(usuario.Id, token);

                return Ok(new { mensaje = "Usuario autenticado con éxito", data = usuario.FromModelToLoginResponse() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al autenticar el usuario.");
                return StatusCode(500, new { mensaje = "Ocurrió un error al procesar su solicitud." });
            }
        }


        [HttpPost("confirmar-cuenta")]
        public async Task<IActionResult> ConfirmarCuenta([FromHeader(Name = "Authorization")] string authorization)
        {
            if (string.IsNullOrWhiteSpace(authorization))
            {
                return BadRequest("Token es nulo o está vacío.");
            }

            try
            {
                _logger.LogInformation($"Token recibido para confirmación: {authorization}");

                var userIdString = await _usuarioService.GetUserIdByTokenAsync(authorization);

                if (!int.TryParse(userIdString, out int userId) || userId <= 0)
                {
                    return BadRequest("ID de usuario no válido.");
                }

                _logger.LogInformation($"ID de usuario obtenido: {userId}");


                var user = await _usuarioService.GetByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("No se encontró el usuario.");
                }

                user.Verificado = true;
                await _usuarioService.UpdateUserAsync(user);

                return Ok("Cuenta confirmada exitosamente.");
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Error al confirmar la cuenta: Token inválido.");
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al confirmar la cuenta.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPost("solicitar-restablecimiento")]
        public async Task<IActionResult> SolicitarRestablecimiento([FromBody] SolicitarRestablecimientoRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuario = await _usuarioService.GetUserByEmailAsync(requestDto.Email);
                if (usuario == null)
                {
                    return NotFound("No se encontró un usuario con ese correo.");
                }

                var token = await _usuarioService.GenerateResetTokenAsync(usuario);
                if (string.IsNullOrWhiteSpace(token))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo generar el token de restablecimiento.");
                }

                string resetUrl = $"{Request.Scheme}://{Request.Host}/api/Usuario/restablecer-actualizar?token={token}";

                var valoresReemplazo = new Dictionary<string, string>
        {
            { "Nombre", usuario.Nombre },
            { "UrlRestablecimiento", resetUrl }
        };
                string contenidoPlantilla = await _templateService.ObtenerContenidoTemplateAsync("Restablecer", valoresReemplazo);


                Correo correo = new Correo()
                {
                    Para = usuario.Email,
                    Asunto = "Restablecimiento de contraseña",
                    Contenido = contenidoPlantilla
                };

                bool enviado = await CorreoServicio.EnviarAsync(correo);
                if (enviado)
                {
                    return Ok(new { mensaje = "Se ha enviado un correo para restablecer la contraseña." });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al enviar el correo de restablecimiento.");
                }
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Error al solicitar el restablecimiento de contraseña: " + argEx.Message);
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al solicitar el restablecimiento de contraseña.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        [HttpPost("Restablecer-Actualizar")]
        public async Task<IActionResult> RestablecerActualizar([FromBody] RestablecerActualizarRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            if (string.IsNullOrWhiteSpace(requestDto.Token))
            {
                return BadRequest("El token no puede estar vacío.");
            }

            try
            {

                var actualizado = await _usuarioService.RestablecerActualizarAsync(1, requestDto.Password, requestDto.Token);

                if (actualizado)
                {
                    return Ok(new { mensaje = "Contraseña actualizada con éxito." });
                }
                else
                {
                    return BadRequest("No se pudo actualizar la contraseña. Verifica el token.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la contraseña.");
                return StatusCode(500, new { mensaje = "Error interno del servidor." });
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
