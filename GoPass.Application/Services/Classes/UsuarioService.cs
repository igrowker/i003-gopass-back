using GoPass.Application.Services.Interfaces;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Net;

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
            usuario.Password = _passwordHasher.HashPassword(usuario, usuario.Password);

            var userToken = _tokenService.CreateToken(usuario);
            usuario.Token = userToken;

            return await _usuarioRepository.Create(usuario);
        }

        public async Task<Usuario> AuthenticateAsync(string email, string password)
        {
            var userInDb = await _usuarioRepository.GetUserByEmail(email);

            var passwordVerification = _passwordHasher.VerifyHashedPassword(userInDb, userInDb.Password, password);

            if (passwordVerification == PasswordVerificationResult.Failed) throw new Exception("Las credenciales no son correctas");

            var user = await _usuarioRepository.AuthenticateUser(email, password);

            var token = _tokenService.CreateToken(user);
            user.Token = token;

            return user;
        }

        public async Task<bool> VerifyEmailExistsAsync(string email)
        {
            var userEmail = await _usuarioRepository.VerifyEmailExists(email);

            return userEmail!;
        }

        public async Task<bool> VerifyDniExistsAsync(string dni)
        {
            var encriptedDni = _aesGcmCryptoService.Encrypt(dni);
            var userDni = await _usuarioRepository.VerifyDniExists(encriptedDni);

            return userDni;
        }
        public async Task<bool> VerifyPhoneNumberExistsAsync(string phoneNumber)
        {
            var encriptedPhoneNumber = _aesGcmCryptoService.Encrypt(phoneNumber);
            var userPhoneNumber = await _usuarioRepository.VerifyPhoneNumberExists(encriptedPhoneNumber);

            return userPhoneNumber;
        }

        public async Task<string> GetUserIdByTokenAsync(string token)
        {
            var cleanToken = token.StartsWith("Bearer ") ? token.Substring("Bearer ".Length) : null;

            if (cleanToken is null) throw new Exception("Token nulo");

            var decodedToken = await _tokenService.DecodeToken(cleanToken!);

            return decodedToken;
        }
    }
}
