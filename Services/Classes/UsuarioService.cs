using Microsoft.AspNetCore.Identity;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;
using template_csharp_dotnet.Services.Interfaces;

namespace template_csharp_dotnet.Services.Classes
{
    public class UsuarioService : GenericService<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        
        public UsuarioService(IUsuarioRepository usuarioRepository, ITokenService tokenService) : base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
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
            usuario.Password = _usuarioRepository.HashPassword(usuario, usuario.Password);

            var userToken = _tokenService.CreateToken(usuario);
            usuario.Token = userToken;

            return await _usuarioRepository.Create(usuario);
        }

        public async Task<Usuario> AuthenticateAsync(string email, string password)
        {
            var user = await _usuarioRepository.AuthenticateUser(email, password);

            var token = _tokenService.CreateToken(user);
            user.Token = token;

            return user;
        }

        public string HasPassword(Usuario usuario, string password)
        {
            var hashedPassword = _usuarioRepository.HashPassword(usuario, password);
            usuario.Password = hashedPassword;

            return hashedPassword;
        }

        public PasswordVerificationResult VerifyUserPassword(Usuario usuario, string providedPassword)
        {
            return _usuarioRepository.VerifyPassword(usuario, usuario.Password);
        }
    }
}
