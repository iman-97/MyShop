using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockContext<T> : IRepository<T> where T : BaseEntity
    {
        private List<T> _items;
        private string _className;

        public MockContext()
        {
            _items = new List<T>();
        }

        public void Commit()
        {
            return;
        }

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
