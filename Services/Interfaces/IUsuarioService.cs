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
        string HasPassword(Usuario usuario, string password);
        PasswordVerificationResult VerifyUserPassword(Usuario usuario, string providedPassword);
        Task<Usuario> RegisterUserAsync(Usuario usuario);
    }
}
