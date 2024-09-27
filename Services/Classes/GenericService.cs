using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using template_csharp_dotnet.Models;
using template_csharp_dotnet.Repositories.Interfaces;
using template_csharp_dotnet.Services.Interfaces;

namespace template_csharp_dotnet.Services.Classes
{
    public class GenericService<T> : IGenericService<T> where T : BaseModel
    {
        protected readonly IGenericRepository<T> _genericRepository;

        public GenericService(IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _genericRepository.GetAll();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _genericRepository.GetById(id);
        }

        public async Task<T> Create(T model)
        {
            return await _genericRepository.Create(model);
        }
        public async Task<T> Update(int id, T model)
        {
            return await _genericRepository.Update(id, model);
        }

        public async Task<T> Delete(int id)
        {
            return await _genericRepository.Delete(id);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate) => await _genericRepository.FindAsync(predicate);
    }
}
