using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Services.Interfaces
{
    public interface IUsuarioService : IGenericService<Usuario>
    {

        Task<List<Usuario>> GetAllUsersWithRelationsAsync();
        Task<Usuario> DeleteUserWithRelationsAsync(int id);
        Task<Usuario> GetUserByEmail(string email);
    }
}
