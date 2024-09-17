using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Repositories.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario> DeleteUserWithRelations(int id);
        Task<List<Usuario>> GetAllUsersWithRelations();
        Task<Usuario> GetUserByEmail(string email);
        string HashPassword(Usuario usuario, string providedPassword);
        PasswordVerificationResult VerifyPassword(Usuario usuario, string providedPassword);
        Task<Usuario> AuthenticateUser(string email, string password);
    }
}
