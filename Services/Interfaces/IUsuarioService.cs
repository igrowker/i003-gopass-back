using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Services.Interfaces
{
    public interface IUsuarioService : IGenericService<Usuario>
    {

        Task<List<Usuario>> GetAllUsersWithRelationsAsync();
        //Task<Usuario> GetUserByIdAsync(int id);
        //Task<Usuario> CreateUserAsync(Usuario usuario);
        //Task<Usuario> UpdateUserAsync(int id,Usuario usuario);
        Task<Usuario> DeleteUserWithRelationsAsync(int id);
    }
}
