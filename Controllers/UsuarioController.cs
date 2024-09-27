﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using template_csharp_dotnet.DTOs.Request.AuthRequestDTOs;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Services.Interfaces;
using template_csharp_dotnet.Utilities.Mappers;
using template_csharp_dotnet.Validators;

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

                userToRegister.Verificado = false;
                var registeredUser = await _usuarioService.RegisterUserAsync(userToRegister);

                return Ok(registeredUser);
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
                //dbUserCredentials.Nombre = credentialsToModify.Nombre;
                //dbUserCredentials.NumeroTelefono = credentialsToModify.NumeroTelefono;
                //dbUserCredentials.DNI = credentialsToModify.DNI;
        
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
