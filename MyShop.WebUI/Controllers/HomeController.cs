using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> _context;
        IRepository<ProductCategory> _productCategory;

        public HomeController(IRepository<Product> productContext,
            IRepository<ProductCategory> productCategoryContext)
        {
            _context = productContext;
            _productCategory = productCategoryContext;
        }

        public ActionResult Index(string Category = null)
        {
            List<Product> products;
            var categories = _productCategory.Collection().ToList();

            if (Category == null)
                products = _context.Collection().ToList();
            else
                products = _context.Collection().Where(p => p.Category == Category).ToList();

            var model = new ProductListViewModel();
            model.Products = products;
            model.ProductCategories = categories;

            return View(model);
        }

        public ActionResult Details(string id)
        {
            var product = _context.Find(id);

            if (product == null)
                return HttpNotFound();
            else
                return View(product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}