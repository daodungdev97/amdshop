using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class//chỉ ra rằng T là 1 kiểu tham chiếu class, ko phải struct
    {
        //T - Category
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter =null, string? includeProperties = null, bool tracked = false);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
