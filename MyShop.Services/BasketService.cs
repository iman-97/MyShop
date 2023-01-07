using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        public const string BasketSessionName = "eCommerceBasket";

        private readonly IRepository<Product> _productContext;
        private readonly IRepository<Basket> _basketContext;

        public BasketService(IRepository<Product> productContext, IRepository<Basket> basketContext)
        {
            _productContext = productContext;
            _basketContext = basketContext;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            var basket = GetBasket(httpContext, true);
            var item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                basket.BasketItems.Add(item);
            }
            else
                item.Quantity++;

            _basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            var basket = GetBasket(httpContext, true);
            var item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
                _basketContext.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            var basket = GetBasket(httpContext, false);

            if (basket == null)
                return new List<BasketItemViewModel>();

            var results = (from b in basket.BasketItems
                           join p in _productContext.Collection() on b.ProductId equals p.Id
                           select new BasketItemViewModel()
                           {
                               Id = b.Id,
                               Quantity = b.Quantity,
                               ProductName = p.Name,
                               Price = p.Price,
                               Image = p.Image
                           }).ToList();

            return results;   
        }

        public BasketSummaryViewModel GetBasketSummery(HttpContextBase httpContext)
        {
            var basket = GetBasket(httpContext, false);
            var model = new BasketSummaryViewModel(0, 0);

            if (basket == null)
                return model;

            int? bascketCount = (from item in basket.BasketItems
                                 select item.Quantity).Sum();

            decimal? basketTotal = (from item in basket.BasketItems
                                    join p in _productContext.Collection() on item.ProductId equals p.Id
                                    select item.Quantity * p.Price).Sum();

            model.BasketCount = bascketCount ?? 0;
            model.BasketTotal = basketTotal ?? decimal.Zero;

            return model;
        }

        public void ClearBasket(HttpContextBase httpContext)
        {
            var basket = GetBasket(httpContext, false);
            basket.BasketItems.Clear();
            _basketContext.Commit();
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            //getting user cookies
            var cookie = httpContext.Request.Cookies.Get(BasketSessionName);
            var basket = new Basket();

            if (cookie != null)
            {
                var basketId = cookie.Value;
                if (string.IsNullOrEmpty(basketId) == false)
                {
                    basket = _basketContext.Find(basketId);
                }
                else
                {
                    if(createIfNull == true)
                        basket = CreateNewBasket(httpContext);
                }
            }
            else
            {
                if (createIfNull == true)
                    basket = CreateNewBasket(httpContext);
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            var basket = new Basket();
            _basketContext.Insert(basket);
            _basketContext.Commit();

            var cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

    }
}
