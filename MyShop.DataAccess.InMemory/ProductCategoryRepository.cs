using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        private ObjectCache _cache = MemoryCache.Default;
        List<ProductCategory> _productCategories;

        public ProductCategoryRepository()
        {
            _productCategories = _cache["productsCategories"] as List<ProductCategory>;

            if (_productCategories == null)
                _productCategories = new List<ProductCategory>();
        }

        public void Commit() => _cache["productsCategories"] = _productCategories;

        public void Insert(ProductCategory product) => _productCategories.Add(product);

        public void Update(ProductCategory product)
        {
            var productToUpdate = _productCategories.Find(p => p.Id == product.Id);

            if (productToUpdate == null)
                throw new Exception("Product not found");
            else
                productToUpdate = product;
        }

        public ProductCategory Find(string id)
        {
            var product = _productCategories.Find(p => p.Id == id);

            if (product == null)
                throw new Exception("Product not found");
            else
                return product;
        }

        public IQueryable<ProductCategory> Collection() => _productCategories.AsQueryable();

        public void Delete(string id)
        {
            var productToDelete = _productCategories.Find(p => p.Id == id);

            if (productToDelete == null)
                throw new Exception("Product not found");
            else
                _productCategories.Remove(productToDelete);
        }
    }
}
