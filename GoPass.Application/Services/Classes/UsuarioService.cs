using GoPass.Application.Services.Interfaces;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GoPass.Application.Services.Classes
{
    public class UsuarioService : GenericService<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IAesGcmCryptoService _aesGcmCryptoService;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly ILogger<UsuarioService> _logger;
        public UsuarioService(IUsuarioRepository usuarioRepository, ITokenService tokenService, IAesGcmCryptoService aesGcmCryptoService, ILogger<UsuarioService> logger) : base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _aesGcmCryptoService = aesGcmCryptoService;
            _passwordHasher = new PasswordHasher<Usuario>();
            _logger = logger;
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
            var existingUser = await _usuarioRepository.GetUserByEmail(usuario.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("El usuario ya existe.");
            }

            usuario.Password = _passwordHasher.HashPassword(usuario, usuario.Password);

            var createdUser = await _usuarioRepository.Create(usuario);

            var userToken = _tokenService.CreateToken(createdUser);
            createdUser.Token = userToken;

            return createdUser;
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

        public async Task<bool> VerifyDniExistsAsync(string dni)
        {
            string encriptedDni = _aesGcmCryptoService.Encrypt(dni);
            bool userDni = await _usuarioRepository.VerifyDniExists(encriptedDni);

            return userDni;
        }
        public async Task<bool> VerifyPhoneNumberExistsAsync(string phoneNumber)
        {
            string encriptedPhoneNumber = _aesGcmCryptoService.Encrypt(phoneNumber);
            bool userPhoneNumber = await _usuarioRepository.VerifyPhoneNumberExists(encriptedPhoneNumber);

            return userPhoneNumber;
        }

        public async Task<string> GetUserIdByTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token) || !token.StartsWith("Bearer "))
                throw new ArgumentException("Token nulo o mal formado");

            var cleanToken = token.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrWhiteSpace(cleanToken))
                throw new ArgumentException("Token nulo o mal formado");

            var decodedToken = await _tokenService.DecodeToken(cleanToken);

            if (string.IsNullOrWhiteSpace(decodedToken))
                throw new Exception("No se pudo decodificar el token para obtener el ID del usuario");

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

                usuario.Restablecer = 0;
                usuario.Password = _passwordHasher.HashPassword(usuario, nuevaPassword);
                usuario.Token = null;

                await _usuarioRepository.UpdateAsync(usuario);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al restablecer la contraseña.");
                return false;
            }
        }

        public async Task<string> GenerateResetTokenAsync(string email)
        {
            var usuario = await _usuarioRepository.GetUserByEmail(email);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            usuario.Token = Guid.NewGuid().ToString();
            await _usuarioRepository.UpdateAsync(usuario); 
            return usuario.Token;
        }

        public async Task<Usuario> UpdateUserAsync(Usuario usuario)
        {
            return await _usuarioRepository.UpdateAsync(usuario);
        }
        public async Task UpdateUserTokenAsync(int userId, string token)
        {
            var user = await _usuarioRepository.FindAsync(userId);
            if (user != null)
            {
                user.Token = token;
                await _usuarioRepository.UpdateAsync(user);
            }
        }
    }
}
