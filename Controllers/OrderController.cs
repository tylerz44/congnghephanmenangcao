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
        public IActionResult Cancel(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = _context.Orders
                                .Include(o => o.OrderDetails)
                                .FirstOrDefault(o => o.OrderId == id &&
                                                     o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            // Chỉ được hủy khi chưa xác nhận
            if (order.ShippingStatus != "Chờ xác nhận")
            {
                TempData["Error"] = "Đơn hàng đã được xác nhận nên không thể hủy.";
                return RedirectToAction(nameof(History));
            }

            // Trả lại số lượng sản phẩm
            foreach (var item in order.OrderDetails)
            {
                var product = _context.Products.Find(item.ProductId);

                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }
            }

            _context.OrderDetails.RemoveRange(order.OrderDetails);
            _context.Orders.Remove(order);

            _context.SaveChanges();

            TempData["Success"] = "Hủy đơn hàng thành công.";

            return RedirectToAction(nameof(History));
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
