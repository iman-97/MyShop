using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.WebUI;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexPageDoesReturnProduct()
        {
            IRepository<Product> productContext = new MockContext<Product>();
            IRepository<ProductCategory> productCategoryContext = new MockContext<ProductCategory>();
            var controller = new HomeController(productContext, productCategoryContext);

            productContext.Insert(new Product());

            var result = controller.Index() as ViewResult;
            var viewModel = (ProductListViewModel)result.ViewData.Model;

            Assert.AreEqual(1, viewModel.Products.Count());
        }

    }
}
