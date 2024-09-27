﻿using Microsoft.AspNetCore.Identity;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;
using template_csharp_dotnet.Services.Interfaces;

namespace template_csharp_dotnet.Services.Classes
{
    public class UsuarioService : GenericService<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        public UsuarioService(IUsuarioRepository usuarioRepository, ITokenService tokenService) : base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
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
            var userDni = await _usuarioRepository.VerifyDniExists(dni);

            return userDni;
        }
        public async Task<bool> VerifyPhoneNumberExistsAsync(string phoneNumber)
        {
            var userPhoneNumber = await _usuarioRepository.VerifyPhoneNumberExists(phoneNumber);

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
