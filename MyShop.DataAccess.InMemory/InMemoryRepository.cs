using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        private ObjectCache _cache = MemoryCache.Default;
        private List<T> _items;
        private string _className;

        public InMemoryRepository()
        {
            _className = typeof(T).Name;
            _items = _cache[_className] as List<T>;

            if (_items == null)
                _items = new List<T>();
        }

        public void Commit() => _cache[_className] = _items;

        public void Insert(T item) => _items.Add(item);

        public void Update(T item)
        {
            var itemtToUpdate = _items.Find(p => p.Id == item.Id);

            if (itemtToUpdate == null)
                throw new Exception(_className + "not found");
            else
                itemtToUpdate = item;
        }

        public T Find(string id)
        {
            var item = _items.Find(p => p.Id == id);

            if (item == null)
                throw new Exception(_className + "not found");
            else
                return item;
        }

        public IQueryable<T> Collection() => _items.AsQueryable();

        public void Delete(string id)
        {
            var itemToDelete = _items.Find(p => p.Id == id);

            if (itemToDelete == null)
                throw new Exception(_className + "not found");
            else
                _items.Remove(itemToDelete);
        }

    }
}
