using GoPass.Application.Services.Interfaces;
using GoPass.Domain.Models;
using GoPass.Infrastructure.Data;
using GoPass.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace GoPass.Application.Services.Classes
{
    public class UsuarioService : GenericService<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IUsuarioRepository usuarioRepository, ITokenService tokenService, ApplicationDbContext context, ILogger<UsuarioService> logger) : base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<Usuario>();
            _context = context;
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
            var userInDb = await _usuarioRepository.GetUserByEmail(email);

            if (userInDb == null)
                return null;

            var passwordVerification = _passwordHasher.VerifyHashedPassword(userInDb, userInDb.Password, password);

            if (passwordVerification == PasswordVerificationResult.Failed)
                return null;


            var token = _tokenService.CreateToken(userInDb);
            userInDb.Token = token;

            return userInDb;
        }
        public async Task<Usuario> UpdateUserAsync(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return usuario;
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
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Token == token);

                if (usuario == null)
                {
                    return false; 
                }

                usuario.Restablecer = restablecer;

                usuario.Password = _passwordHasher.HashPassword(usuario, nuevaPassword);

                usuario.Token = null;

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync(); 

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al restablecer la contraseña.");
                return false;
            }
        }


        public async Task<string> GenerateResetTokenAsync(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            usuario.Token = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync(); 
            return usuario.Token;
        }

        public async Task UpdateUserTokenAsync(int userId, string token)
        {
            var user = await _context.Usuarios.FindAsync(userId); 
            if (user != null)
            {
                user.Token = token;
                await _context.SaveChangesAsync();
            }
        }



    }
}
