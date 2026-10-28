
using FashionStore.Data;
using FashionStore.Extensions;
using FashionStore.Models;
using FashionStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

        //=========================
        // HIỂN THỊ TRANG THANH TOÁN
        //=========================
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<Cart>("Cart");

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.Total = cart.TotalPrice;

            return View(new CheckoutViewModel());
        }

        //=========================
        // XÁC NHẬN THANH TOÁN
        //=========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CheckoutViewModel model)
        {
            var cart = HttpContext.Session.GetObject<Cart>("Cart");

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            Order order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.Now,
                ReceiverName = model.ReceiverName,
                ReceiverPhone = model.ReceiverPhone,
                ShippingAddress = model.ShippingAddress,
                Note = model.Note,
                PaymentMethod = model.PaymentMethod,
                ShippingStatus = "Chờ xác nhận",
                Total = cart.Items.Sum(x => x.Price * x.Quantity)
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
                    Price = item.Price,
                    Size = item.Size,
                    Color = item.Color
                };

                _context.OrderDetails.Add(detail);

                var product = _context.Products.Find(item.ProductId);

                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content(ex.InnerException?.Message ?? ex.Message);
            }

            HttpContext.Session.Remove("Cart");

            TempData["Success"] = "Đặt hàng thành công!";

            return RedirectToAction(nameof(Success));
        }

        //=========================
        // THÀNH CÔNG
        //=========================
        public IActionResult Success()
        {
            return View();
        }
    }
}