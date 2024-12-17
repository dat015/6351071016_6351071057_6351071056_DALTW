
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TechShop.Data;
using TechShop.Models;
using TechShop.Repository.IRepository;

namespace TechShop.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> _dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this._dbSet = _db.Set<T>(); 
        }

        public IEnumerable<T> getAll()
        {
            IQueryable<T> query = _dbSet;
            return query.ToList();

        }
        public IEnumerable<T> getAll(string includeProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.ToList();
        }
        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);

        }
        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);

        }
    }
}
