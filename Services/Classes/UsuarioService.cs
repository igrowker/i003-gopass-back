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

        public async Task<Usuario> GetUserByEmail(string email)
        {
            return await _usuarioRepository.GetUserByEmail(email);
        }

        public async Task<Usuario> DeleteUserWithRelationsAsync(int id)
        {
            var deletedUser = await _usuarioRepository.DeleteUserWithRelations(id);

            return deletedUser;
        }
    }
}
