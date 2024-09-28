
using GoPass.Domain.Models;

namespace GoPass.Infrastructure.Repositories.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario> DeleteUserWithRelations(int id);
        Task<List<Usuario>> GetAllUsersWithRelations();
        Task<Usuario> GetUserByEmail(string email);
        Task<Usuario> AuthenticateUser(string email, string password);
        Task<bool> VerifyPhoneNumberExists(string phoneNumber);
        Task<bool> VerifyDniExists(string dni);
        Task<bool> VerifyEmailExists(string email);
    }
}
