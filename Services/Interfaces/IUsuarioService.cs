using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetAllUsersAsync();
        Task<Usuario> GetUserByIdAsync(int id);
        Task<Usuario> CreateUserAsync(Usuario usuario);
        Task<Usuario> UpdateUserAsync(Usuario usuario);
        Task<Usuario> DeleteUserAsync(int id);
    }
}
