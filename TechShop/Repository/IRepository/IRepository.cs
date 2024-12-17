using System.Linq.Expressions;

namespace TechShop.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> getAll();
        public IEnumerable<T> getAll(string includeProperties)     
;
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
