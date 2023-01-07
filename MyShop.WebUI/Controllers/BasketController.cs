using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly IRepository<Customer> _customers;

        public BasketController(IBasketService basketService, IOrderService orderService,
            IRepository<Customer> customers)
        {
            _basketService = basketService;
            _orderService = orderService;
            _customers = customers;
        }

        public ActionResult Index()
        {
            var model = _basketService.GetBasketItems(HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string id)
        {
            _basketService.AddToBasket(HttpContext, id);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string id)
        {
            _basketService.RemoveFromBasket(HttpContext, id);
            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketSummery = _basketService.GetBasketSummery(HttpContext);
            return PartialView(basketSummery);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            var customer = _customers.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);

            if (customer == null)
                return RedirectToAction("Error");

            var order = new Order
            {
                FirstName = customer.FirstName,
                SurName = customer.LastName,
                City = customer.City,
                Street = customer.Street,
                Email = customer.Email,
                State = customer.State,
                ZipCode = customer.ZipCode
            };

            return View(order);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout(Order order)
        {
            var basketItems = _basketService.GetBasketItems(HttpContext);
            order.OrderStatus = "Order Created";
            order.Email = User.Identity.Name;

            //process payment

            order.OrderStatus = "Payment Processed";
            _orderService.CreateOrder(order, basketItems);
            _basketService.ClearBasket(HttpContext);

            return RedirectToAction("ThankYou", new { orderId = order.Id });
        }

        public ActionResult ThankYou(string orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }

    }
}