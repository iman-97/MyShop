﻿using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepository _context;

        public ProductManagerController()
        {
            _context = new ProductRepository();
        }

        public ActionResult Index()
        {
            var products = _context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            var product = new Product();
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid == false)
                return View(product);

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
                return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string id)
        {
            if (ModelState.IsValid == false)
                return View(product);

            var productToEdit = _context.Find(id);

            if (productToEdit == null)
                return HttpNotFound();
            else
            {
                productToEdit.Name = product.Name;
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
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