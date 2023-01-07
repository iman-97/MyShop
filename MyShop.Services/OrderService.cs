using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderContext;

        public OrderService(IRepository<Order> orderContext)
        {
            _orderContext = orderContext;
        }

        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems)
        {
            foreach (var item in basketItems)
            {
                baseOrder.OrderItems.Add(new OrderItem
                {
                    ProductId = item.Id,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                });
            }

            _orderContext.Insert(baseOrder);
            _orderContext.Commit();
        }

        public List<Order> GetOrderList() => _orderContext.Collection().ToList();

        public Order GetOrder(string id) => _orderContext.Find(id);

        public void UpdateOrder(Order order)
        {
            _orderContext.Update(order);
            _orderContext.Commit();
        }



    }
}
