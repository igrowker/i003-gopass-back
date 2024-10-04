using GoPass.Application.Services.Interfaces;
using GoPass.Application.Utilities.Mappers;
using GoPass.Application.Validators.Users;
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
        private readonly IEmailService _emailService;
        private readonly ModifyUserValidator _modifyUserValidator;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioService usuarioService, 
            IAesGcmCryptoService aesGcmCryptoService, IVonageSmsService vonageSmsService, IEmailService emailService)
        {
            _usuarioService = usuarioService;
            _aesGcmCryptoService = aesGcmCryptoService;
            _vonageSmsService = vonageSmsService;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                Usuario userToRegister = registerRequestDto.FromRegisterToModel();

                userToRegister.Verificado = false;
                Usuario registeredUser = await _usuarioService.RegisterUserAsync(userToRegister);

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
                Usuario userToLogin = loginRequestDto.FromLoginToModel();

                Usuario logUser = await _usuarioService.AuthenticateAsync(userToLogin.Email, userToLogin.Password);

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

                //await _usuarioService.VerifyDniExistsAsync(modifyUsuarioRequestDto.DNI, userId);
                //await _usuarioService.VerifyPhoneNumberExistsAsync(modifyUsuarioRequestDto.DNI, userId);

                if (await _usuarioService.VerifyDniExistsAsync(modifyUsuarioRequestDto.DNI, userId))
                {
                    return BadRequest("El DNI ya se encuentra registrado por otro usuario.");
                }

                // Verificar duplicado de Número de teléfono
                if (await _usuarioService.VerifyPhoneNumberExistsAsync(modifyUsuarioRequestDto.NumeroTelefono, userId))
                {
                    return BadRequest("El número de teléfono ya se encuentra registrado por otro usuario.");
                }

                Usuario credentialsToModify = modifyUsuarioRequestDto.FromModifyUsuarioRequestToModel(dbExistingUserCredentials);


                credentialsToModify.DNI = _aesGcmCryptoService.Encrypt(credentialsToModify.DNI!);
                credentialsToModify.NumeroTelefono = _aesGcmCryptoService.Encrypt(credentialsToModify.NumeroTelefono!);

                Usuario modifiedCredentials = await _usuarioService.Update(userId, credentialsToModify);

                if(modifiedCredentials.DNI is not null && modifiedCredentials.Nombre is not null && modifiedCredentials.NumeroTelefono is not null)
                {
                    modifiedCredentials.Verificado = true;
                }

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