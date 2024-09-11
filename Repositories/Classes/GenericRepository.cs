using Microsoft.EntityFrameworkCore;
using template_csharp_dotnet.Data;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;

namespace template_csharp_dotnet.Repositories.Classes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        protected DbSet<T> _dbSet;
        protected DbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
            _dbContext = dbContext;
        }
        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            var recordDb = await _dbSet.FindAsync(id);
            if (recordDb is null) throw new Exception($"No se encontro el ID: {id}");
            return recordDb;
        }

        public async Task<T> Create(T model)
        {
            _dbSet.Add(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }
        public Task<T> Update(T model)
        {
            throw new NotImplementedException(); //COMPLETAR
        }

        public Task<T> Delete(int id)
        {
            throw new NotImplementedException(); //COMPLETAR
        }

      
    }
}
