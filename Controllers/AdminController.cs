using FashionStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        public IActionResult Dashboard()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            ViewBag.TotalProducts = _context.Products.Count();
            ViewBag.TotalCustomers = _context.UserAccounts.Count(x => x.Role == "Customer");
            ViewBag.TotalOrders = _context.Orders.Count();
            ViewBag.TotalRevenue = _context.Orders.Sum(x => x.Total);

            return View();
        }

        public IActionResult Products()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var list = _context.Products
                               .Include(x => x.Category)
                               .ToList();

            return View(list);
        }

        public IActionResult Categories()
        {
            return View(_context.Categories.ToList());
        }

		public IActionResult Orders()
		{
			var list = _context.Orders.ToList();

			return View(list);
		}
        public IActionResult OrderDetail(int id)
        {
            var details = _context.OrderDetails
                .Where(x => x.OrderId == id)
                .ToList();

            return View(details);
        }
        public IActionResult UpdateOrder(int id)
		{
			var order = _context.Orders.Find(id);

			if (order == null)
				return NotFound();

			switch (order.ShippingStatus)
			{
				case "Chờ xác nhận":
					order.ShippingStatus = "Đã xác nhận";
					break;

				case "Đã xác nhận":
					order.ShippingStatus = "Đang giao";
					break;

				case "Đang giao":
					order.ShippingStatus = "Hoàn thành";
					break;
			}

			_context.SaveChanges();

			return RedirectToAction(nameof(Orders));
		}
	


		public IActionResult Customers(string search)
        {
            var list = _context.UserAccounts
                .Where(x => x.Role == "Customer");

            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(x =>
                    x.FullName.Contains(search) ||
                    x.Email.Contains(search));
            }

            return View(list.ToList());
        }
        public IActionResult ToggleUser(int id)
        {
            var user = _context.UserAccounts.Find(id);

            if (user == null)
                return NotFound();

            user.IsActive = !user.IsActive;

            _context.SaveChanges();

            return RedirectToAction(nameof(Customers));
        }

        public IActionResult Revenue()
		{
			var orders = _context.Orders.ToList();

			return View(orders);
		}

        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Settings(
            string StoreName,
            string Email,
            string Phone,
            string Address,
            string Facebook,
            string Instagram,
            string TikTok)
        {
            TempData["Success"] = "Lưu cài đặt thành công.";

            return RedirectToAction(nameof(Settings));
        }
    }
}