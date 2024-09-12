using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetAllUsers();
        Task<Usuario> GetUserById(int id);
        Task<Usuario> CreateUser(Usuario usuario);
        Task<Usuario> UpdateUser(Usuario usuario);
        Task<Usuario> DeleteUser(int id);
    }
}
