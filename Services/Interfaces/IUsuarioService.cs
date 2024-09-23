using Microsoft.AspNetCore.Identity;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Services.Interfaces
{
    public interface IUsuarioService : IGenericService<Usuario>
    {

        Task<List<Usuario>> GetAllUsersWithRelationsAsync();
        Task<Usuario> DeleteUserWithRelationsAsync(int id);
        Task<Usuario> GetUserByEmailAsync(string email);
        Task<Usuario> AuthenticateAsync(string email, string password);
        Task<Usuario> RegisterUserAsync(Usuario usuario);
        Task<bool> VerifyUserCredentialsAsync(string dni, string phoneNumber, string email);
        Task<string> GetUserIdByTokenAsync(string token);
    }
}
