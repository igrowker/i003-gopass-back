﻿using GoPass.Application.Services.Interfaces;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Vonage.Users;

namespace GoPass.Application.Services.Classes
{
    public class UsuarioService : GenericService<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IAesGcmCryptoService _aesGcmCryptoService;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        public UsuarioService(IUsuarioRepository usuarioRepository, ITokenService tokenService, IAesGcmCryptoService aesGcmCryptoService) : base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _aesGcmCryptoService = aesGcmCryptoService;
            _passwordHasher = new PasswordHasher<Usuario>();    
        }
        public async Task<List<Usuario>> GetAllUsersWithRelationsAsync()
        {
            var users = await _usuarioRepository.GetAllUsersWithRelations();

            return users;
        }

        public async Task<Usuario> GetUserByEmailAsync(string email)
        {
            return await _usuarioRepository.GetUserByEmail(email);
        }

        public async Task<Usuario> DeleteUserWithRelationsAsync(int id)
        {
            var deletedUser = await _usuarioRepository.DeleteUserWithRelations(id);

            return deletedUser;
        }

        public async Task<Usuario> RegisterUserAsync(Usuario usuario)
        {
            // Primero, hashea la contraseña del usuario.
            usuario.Password = _passwordHasher.HashPassword(usuario, usuario.Password);

            // Guarda el usuario en la base de datos. Esto generará el ID.
            var nuevoUsuario = await _usuarioRepository.Create(usuario);

            // Asegúrate de que el nuevoUsuario ahora tiene un ID válido.
            if (nuevoUsuario.Id <= 0)
            {
                throw new Exception("El ID del usuario no es válido después de la creación.");
            }

            // Genera el token ahora que tienes un usuario persistido con un ID válido.
            var userToken = _tokenService.CreateToken(nuevoUsuario);
            nuevoUsuario.Token = userToken; // Si deseas almacenar el token en el objeto usuario.
            await _usuarioRepository.StorageToken(usuario.Id, userToken);
            return nuevoUsuario; // Devuelve el usuario con el token si es necesario.

        }


        public async Task<Usuario> AuthenticateAsync(string email, string password)
        {
            Usuario userInDb = await _usuarioRepository.GetUserByEmail(email);

            PasswordVerificationResult passwordVerification = _passwordHasher.VerifyHashedPassword(userInDb, userInDb.Password, password);

            if (passwordVerification == PasswordVerificationResult.Failed) throw new Exception("Las credenciales no son correctas");

            Usuario user = await _usuarioRepository.AuthenticateUser(email, password);

            string token = _tokenService.CreateToken(user);
            user.Token = token;

            return user;
        }

        public async Task<bool> VerifyEmailExistsAsync(string email)
        {
            bool userEmail = await _usuarioRepository.VerifyEmailExists(email);

            return userEmail!;
        }

        public async Task<bool> VerifyDniExistsAsync(string dni, int userId)
        {
            string encriptedDni = _aesGcmCryptoService.Encrypt(dni);
            bool userDni = await _usuarioRepository.VerifyDniExists(encriptedDni, userId);

            return userDni;
        }
        public async Task<bool> VerifyPhoneNumberExistsAsync(string phoneNumber, int userId)
        {
            string encriptedPhoneNumber = _aesGcmCryptoService.Encrypt(phoneNumber);
            bool userPhoneNumber = await _usuarioRepository.VerifyPhoneNumberExists(encriptedPhoneNumber, userId);

            return userPhoneNumber;
        }

        public async Task<string> GetUserIdByTokenAsync(string token)
        {
            // Verifica si el token tiene el prefijo "Bearer "
            string cleanToken = token.StartsWith("Bearer ") ? token.Substring("Bearer ".Length) : token;

            if (string.IsNullOrWhiteSpace(cleanToken))
            {
                throw new Exception("Token nulo o vacío.");
            }

            // Decodifica el token
            string decodedToken = await _tokenService.DecodeToken(cleanToken!);

            return decodedToken;
        }


        public async Task<bool> RestablecerActualizarAsync(int restablecer, string nuevaPassword, string token)
        {
            try
            {
                var usuario = await _usuarioRepository.GetUserByToken(token);
                
                if (usuario == null)
                {
                    return false;
                }

                usuario.Restablecer = true;
                usuario.Password = _passwordHasher.HashPassword(usuario, nuevaPassword);
                usuario.Token = null;

                await _usuarioRepository.Update(usuario.Id, usuario);
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error al restablecer la contraseña.");
                return false;
            }
        }

        public async Task<bool> ValidateUserCredentialsToPublishTicket(int userId)
        {
            bool isvalid = true;

            Usuario usuario = await _usuarioRepository.GetById(userId);


            if (string.IsNullOrEmpty(usuario.Nombre) ||
            string.IsNullOrEmpty(usuario.DNI) ||
            string.IsNullOrEmpty(usuario.NumeroTelefono) ||
            !usuario.VerificadoEmail ||
            !usuario.VerificadoSms)
            {
                return isvalid = false;
            }

            return isvalid;
        }

    }
}
