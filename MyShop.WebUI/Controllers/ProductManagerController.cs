using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> _context;
        IRepository<ProductCategory> _productCategory;

        public ProductManagerController(IRepository<Product> productContext,
            IRepository<ProductCategory> productCategoryContext)
        {
            _context = productContext;
            _productCategory = productCategoryContext;
        }

        public ActionResult Index()
        {
            var products = _context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            var viewModel = new ProductManagerViewModel();
            viewModel.Product = new Product();
            viewModel.ProductCategories = _productCategory.Collection();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid == false)
                return View(product);

            if (file != null)
            {
                product.Image = product.Id + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
            }

            _context.Insert(product);
            _context.Commit();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var product = _context.Find(id);

            if (product == null)
                return HttpNotFound();
            else
            {
                var viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = _productCategory.Collection();

                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, string id, HttpPostedFileBase file)
        {
            if (ModelState.IsValid == false)
                return View(product);

            var productToEdit = _context.Find(id);

            if (productToEdit == null)
                return HttpNotFound();
            else
            {
                if (file != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                }

                productToEdit.Name = product.Name;
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Price = product.Price;

                _context.Commit();

                return RedirectToAction("Index");
            }

        }

        public ActionResult Delete(string id)
        {
            var productToDelete = _context.Find(id);

            if (productToDelete == null)
                return HttpNotFound();
            else
                return View(productToDelete);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            var productToDelete = _context.Find(id);

            if (productToDelete == null)
                return HttpNotFound();
            else
            {
                _context.Delete(id);
                _context.Commit();
                return RedirectToAction("Index");
            }
        }

    }
}