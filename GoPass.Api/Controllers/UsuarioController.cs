using GoPass.Application.Services.Classes;
using GoPass.Application.Services.Interfaces;
using GoPass.Application.Utilities.Mappers;
using GoPass.Domain.DTOs.Request.AuthRequestDTOs;
using GoPass.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoPass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAesGcmCryptoService _aesGcmCryptoService;
        private readonly IVonageSmsService _vonageSmsService;
        private readonly ILogger<UsuarioController> _logger;
        private readonly ITemplateService _templateService;
        private readonly ITokenService _tokenService;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioService usuarioService, 
            IAesGcmCryptoService aesGcmCryptoService, IVonageSmsService vonageSmsService, ITemplateService templateService, ITokenService tokenService)
        {
            _templateService = templateService;
            _tokenService = tokenService;
            _usuarioService = usuarioService;
            _aesGcmCryptoService = aesGcmCryptoService;
            _vonageSmsService = vonageSmsService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userToRegister = registerRequestDto.FromRegisterToModel();
                userToRegister.VerificadoEmail = false;

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


                if (!usuario.VerificadoEmail)
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

                user.VerificadoEmail = true;
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


                var token = await _usuarioService.GenerateResetTokenAsync(usuario.Email);
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

        [HttpPost("restablecer-actualizar")]
        public async Task<IActionResult> RestablecerActualizar([FromBody] RestablecerActualizarPasswordRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(requestDto.Token))
            {
                return BadRequest(new { mensaje = "El token no puede estar vacío." });
            }

            try
            {
                var actualizado = await _usuarioService.RestablecerActualizarAsync(0, requestDto.Password, requestDto.Token);

                if (actualizado)
                {
                    return Ok(new { mensaje = "Contraseña actualizada con éxito." });
                }
                else
                {
                    return BadRequest(new { mensaje = "No se pudo actualizar la contraseña. Verifica el token o el estado de la solicitud." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la contraseña.");
                return StatusCode(500, new { mensaje = "Error interno del servidor." });
            }
        }


        [Authorize]
        [HttpGet("user-credentials")]
        public async Task<IActionResult> GetUserCredentials()
        {
            string authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            string userIdObtainedString = await _usuarioService.GetUserIdByTokenAsync(authHeader);
            int userId = int.Parse(userIdObtainedString);
            Usuario dbExistingUserCredentials = await _usuarioService.GetByIdAsync(userId);

            dbExistingUserCredentials.DNI = _aesGcmCryptoService.Decrypt(dbExistingUserCredentials.DNI!);
            dbExistingUserCredentials.NumeroTelefono = _aesGcmCryptoService.Decrypt(dbExistingUserCredentials.NumeroTelefono!);

            return Ok(dbExistingUserCredentials);
        }

        [Authorize]
        [HttpPut("modify-user-credentials")]
        public async Task<IActionResult> ModifyUserCredentials(ModifyUsuarioRequestDto modifyUsuarioRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                string authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                string userIdObtainedString = await _usuarioService.GetUserIdByTokenAsync(authHeader);
                int userId = int.Parse(userIdObtainedString);
                Usuario dbExistingUserCredentials = await _usuarioService.GetByIdAsync(userId);

                Usuario credentialsToModify = modifyUsuarioRequestDto.FromModifyUsuarioRequestToModel(dbExistingUserCredentials);


                credentialsToModify.DNI = _aesGcmCryptoService.Encrypt(credentialsToModify.DNI!);
                credentialsToModify.NumeroTelefono = _aesGcmCryptoService.Encrypt(credentialsToModify.NumeroTelefono!);

                Usuario modifiedCredentials = await _usuarioService.Update(userId, credentialsToModify);

                return Ok(modifiedCredentials);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("verify-phone")]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var result = await _vonageSmsService.SendVonageVerificationCode(phoneNumber);

            if (result)
            {
                return Ok(new { message = "Código de verificación enviado exitosamente." });
            }

            return BadRequest(new { message = "Error al enviar el código de verificación." });
        }

        [HttpPost("verify-provided-code")]
        public async Task<IActionResult> VerifyVonageCodeProvided(int vonageCode)
        {
            bool code = _vonageSmsService.VerifyCode(vonageCode);

            return Ok("Se verifico su numero de telefono correctamente" + code);
        }
    }
}