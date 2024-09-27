using System.Linq.Expressions;
using template_csharp_dotnet.Models;

namespace template_csharp_dotnet.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Create(T model);
        Task<T> Update(int id, T model);
        Task<T> Delete(int id);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);

    }
}
