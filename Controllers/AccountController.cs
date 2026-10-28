using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserAccount user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            bool exists = _context.UserAccounts
                                  .Any(x => x.Email == user.Email);

            if (exists)
            {
                ViewBag.Error = "Email đã tồn tại!";
                return View(user);
            }

            user.Role = "Customer";
            user.IsActive = true;

            _context.UserAccounts.Add(user);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công. Vui lòng đăng nhập!";

            return RedirectToAction(nameof(Login));
        }

       
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            var user = _context.UserAccounts.FirstOrDefault(x =>
                x.Email == email &&
                x.Password == password &&
                x.IsActive);

            if (user == null)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng!";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", user.Role);

            TempData["Success"] = "Đăng nhập thành công!";

            return RedirectToAction("Index", "Home");
        }

        

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["Success"] = "Đăng xuất thành công!";

            return RedirectToAction(nameof(Login));
        }

       

        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = _context.UserAccounts
                               .FirstOrDefault(x => x.UserId == userId);

            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View(user);
        }

     

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(UserAccount model)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = _context.UserAccounts
                               .FirstOrDefault(x => x.UserId == userId);

            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            user.FullName = model.FullName;
            user.Phone = model.Phone;
            user.Address = model.Address;

            _context.SaveChanges();

            HttpContext.Session.SetString("FullName", user.FullName);

            TempData["Success"] = "Cập nhật thông tin thành công!";

            return RedirectToAction(nameof(Profile));
        }
    }
}