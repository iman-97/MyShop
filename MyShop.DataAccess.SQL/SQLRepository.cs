using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext _dataContext;
        internal DbSet<T> _dbSet;

        public SQLRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _dbSet = _dataContext.Set<T>();
        }

        public IQueryable<T> Collection() => _dbSet;

        public void Commit() => _dataContext.SaveChanges();

        public void Delete(string id)
        {
            var item = Find(id);

            if (_dataContext.Entry(item).State == EntityState.Detached)
                _dbSet.Attach(item);

            _dbSet.Remove(item);
        }

        public T Find(string id) => _dbSet.Find(id);

        public void Insert(T item) => _dbSet.Add(item);

        public void Update(T item)
        {
            _dbSet.Attach(item);
            _dataContext.Entry(item).State = EntityState.Modified;
        }
    }
}
