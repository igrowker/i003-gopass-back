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
        public Task<List<T>> GetAllAsync()
        {
            return _genericRepository.GetAll();
        }

        public Task<T> GetByIdAsync(int id)
        {
            return _genericRepository.GetById(id);
        }

        public Task<T> Create(T model)
        {
            return _genericRepository.Create(model);
        }
        public Task<T> Update(int id, T model)
        {
            return _genericRepository.Update(id, model);
        }

        public Task<T> Delete(int id)
        {
            return _genericRepository.Delete(id);
        }


    }
}
