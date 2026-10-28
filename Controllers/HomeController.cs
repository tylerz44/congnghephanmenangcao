using FashionStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                                   .Include(x => x.Category)
                                   .OrderByDescending(x => x.ProductId)
                                   .ToList();

            ViewBag.Categories = _context.Categories.ToList();

            return View(products);
        }

        public IActionResult Men()
        {
            var products = _context.Products
                                   .Include(x => x.Category)
                                   .Where(x => x.Gender == "Nam")
                                   .ToList();

            ViewBag.Categories = _context.Categories.ToList();

            return View("Index", products);
        }

        public IActionResult Women()
        {
            var products = _context.Products
                                   .Include(x => x.Category)
                                   .Where(x => x.Gender == "Nữ")
                                   .ToList();

            ViewBag.Categories = _context.Categories.ToList();

            return View("Index", products);
        }
    }
}