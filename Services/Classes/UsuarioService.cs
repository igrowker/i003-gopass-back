using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;
using template_csharp_dotnet.Services.Interfaces;

namespace template_csharp_dotnet.Services.Classes
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)  
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<List<Usuario>> GetAllUsersAsync()
        {
            var users = await _usuarioRepository.GetAll();

            return users;
        }
        public async Task<Usuario> GetUserByIdAsync(int id)
        {
            var user = await _usuarioRepository.GetById(id);

            return user;
        }

        public async Task<Usuario> CreateUserAsync(Usuario usuario)
        {
            var userToCreate = await _usuarioRepository.Create(usuario);

            return userToCreate;
        }
        public Task<Usuario> UpdateUserAsync(Usuario usuario)
        {
            var userToUpdate = _usuarioRepository.Update(usuario);

            return userToUpdate;
        }

        public Task<Usuario> DeleteUserAsync(int id)
        {
            var deletedUser = _usuarioRepository.Delete(id);

            return deletedUser;
        }
    }
}
