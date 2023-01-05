using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        private ObjectCache _cache = MemoryCache.Default;
        List<Product> _products;

        public ProductRepository()
        {
            _products = _cache["products"] as List<Product>;

            if (_products == null)
                _products = new List<Product>();
        }

        public void Commit() => _cache["products"] = _products;

        public void Insert(Product product) => _products.Add(product);

        public void Update(Product product)
        {
            var productToUpdate = _products.Find(p => p.Id == product.Id);

            if (productToUpdate == null)
                throw new Exception("Product not found");
            else
                productToUpdate = product;
        }

        public Product Find(string id)
        {
            var product = _products.Find(p => p.Id == id);

            if(product == null)
                throw new Exception("Product not found");
            else
                return product;
        }

        public IQueryable<Product> Collection() => _products.AsQueryable();

        public void Delete(string id)
        {
            var productToDelete = _products.Find(p => p.Id == id);

            if (productToDelete == null)
                throw new Exception("Product not found");
            else
                _products.Remove(productToDelete);
        }

    }
}
