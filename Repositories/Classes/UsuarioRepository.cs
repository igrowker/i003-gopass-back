using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using template_csharp_dotnet.Data;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;

namespace template_csharp_dotnet.Repositories.Classes
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        //private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            //_passwordHasher = new PasswordHasher<Usuario>();
        }

        public async Task<List<Usuario>> GetAllUsersWithRelations()
        {
            return await _dbSet.Include(x => x.Reventa!).ThenInclude(x => x.Entrada).ToListAsync();
        }

        public async Task<Usuario> GetUserByEmail(string email)
        {
            var emailExists = await _dbSet.FirstOrDefaultAsync(x => x.Email == email);

            //if (emailExists is not null)
            //{
            //    throw new Exception($"El email: {email} ya se encuentra registrado");
            //}

            return emailExists!;
        }

        public async Task<Usuario> DeleteUserWithRelations(int id)
        {
            var recordToDelete = await GetById(id);

            if (recordToDelete is null) throw new Exception("El registro no se encontro");

            await _dbSet.Where(x => x.Id == id).Include(x => x.Entrada!).ThenInclude(x => x.Reventa).ExecuteDeleteAsync();

            return recordToDelete;
        }

        public async Task<Usuario> AuthenticateUser(string email, string password)
        {
            var userToAuthenticate = await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

            if (userToAuthenticate is null) throw new Exception("Ha habido un error, verifique los campos e intentelo nuevamente");

            return userToAuthenticate;
        }

        public async Task<bool> VerifyUserCredentials(string dni, string phoneNumber, string email)
        {
            var userCredentialsExist = await _dbSet.AnyAsync(u => u.DNI == dni || u.NumeroTelefono == phoneNumber || u.Email == email);

            return userCredentialsExist;
        }
    }
}
