using FashionStore.Data;
using FashionStore.Extensions;
using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Checkout()
        {
            Cart cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();

            return View(cart);
        }
        [HttpPost]
        public IActionResult Checkout(
string receiverName,
string phone,
string address,
string note,
string payment)
        {

            Cart cart = HttpContext.Session.GetObject<Cart>("Cart");

            if (cart == null)
                return RedirectToAction("Index", "Cart");

            Order order = new Order
            {
                ReceiverName = receiverName,
                ReceiverPhone = phone,
                ShippingAddress = address,
                Note = note,
                PaymentMethod = payment,
                Total = cart.TotalPrice,

                ShippingStatus = "Chờ xác nhận",

                UserId = 1
            };

            _context.Orders.Add(order);

            _context.SaveChanges();

            foreach (var item in cart.Items)
            {
                OrderDetail detail = new OrderDetail
                {
                    OrderId = order.OrderId,

                    ProductId = item.ProductId,

                    Quantity = item.Quantity,

                    Price = item.Price
                };

                _context.OrderDetails.Add(detail);
            }

            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");

            return RedirectToAction(nameof(Success));

        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult History()
        {
            var orders = _context.Orders
                                 .OrderByDescending(o => o.OrderDate)
                                 .ToList();

            return View(orders);
        }
        public IActionResult Detail(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
