using FashionStore.Data;
using FashionStore.Extensions;
using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
 
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddToCart(int id, string size, string color, int quantity)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == id);

            if (product == null)
                return NotFound();

            if (quantity <= 0)
                quantity = 1;

            if (string.IsNullOrWhiteSpace(size))
            {
                size = (product.Size ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault() ?? "";
            }

            if (string.IsNullOrWhiteSpace(color))
            {
                color = (product.Color ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault() ?? "";
            }

            Cart cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();

            var item = cart.Items.FirstOrDefault(x =>
                x.ProductId == id &&
                x.Size == size &&
                x.Color == color);

            if (item == null)
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = quantity,
                    Size = size,
                    Color = color,
                    ImageUrl = product.ImageUrl
                });
            }
            else
            {
                item.Quantity += quantity;
            }

            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            Cart cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();
            return View(cart);
        }
        public IActionResult Increase(int id)
        {
            Cart cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();

            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
            }

            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Decrease(int id)
        {
            Cart cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();

            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    cart.Items.Remove(item);
                }
            }

            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int id)
        {
            Cart cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();

            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                cart.Items.Remove(item);
            }

            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            return RedirectToAction(nameof(Index));
        }
    }
}