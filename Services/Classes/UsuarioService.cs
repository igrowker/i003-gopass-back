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
        public async Task<List<Usuario>> GetAllUsers()
        {
            var users = await _usuarioRepository.GetAll();

            return users;
        }
        public async Task<Usuario> GetUserById(int id)
        {
            var user = await _usuarioRepository.GetById(id);

            return user;
        }

        public async Task<Usuario> CreateUser(Usuario usuario)
        {
            var userToCreate = await _usuarioRepository.Create(usuario);

            return userToCreate;
        }
        public Task<Usuario> UpdateUser(Usuario usuario)
        {
            var userToUpdate = _usuarioRepository.Update(usuario);

            return userToUpdate;
        }

        public Task<Usuario> DeleteUser(int id)
        {
            var deletedUser = _usuarioRepository.Delete(id);

            return deletedUser;
        }
    }
}
