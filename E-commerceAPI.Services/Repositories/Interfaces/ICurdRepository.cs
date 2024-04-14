using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Services.Repositories.Interfaces
{
    public interface ICurdRepository<T> where T : class
    {
        Task<int> Add(T Model);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllInclude(params Expression<Func<T, object>>[] includes);
        Task<T>? GetByID(int id);
        Task<T>? GetByName(Expression<Func<T, bool>> expression);
        Task<int> Update(T Model, int id);
        Task<int> Delete(int id);
        Task<T> IsExist(Expression<Func<T, bool>> expression);
    }
}
