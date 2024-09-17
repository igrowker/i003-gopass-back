using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;
using template_csharp_dotnet.Services.Interfaces;

namespace template_csharp_dotnet.Services.Classes
{
    public class UsuarioService : GenericService<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<List<Usuario>> GetAllUsersWithRelationsAsync()
        {
            var users = await _usuarioRepository.GetAllUsersWithRelations();

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
        public async Task<Usuario> UpdateUserAsync(int id, Usuario usuario)
        {
            var userToUpdate = await _usuarioRepository.Update(id, usuario);

            return userToUpdate;
        }

        public async Task<Usuario> DeleteUserWithRelationsAsync(int id)
        {
            var deletedUser = await _usuarioRepository.DeleteUserWithRelations(id);

            return deletedUser;
        }
    }
}
