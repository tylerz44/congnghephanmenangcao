using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;


namespace FashionStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        //=============================
        // KIỂM TRA QUYỀN ADMIN
        //=============================
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        //=============================
        // DANH SÁCH SẢN PHẨM
        //=============================
        public IActionResult Index(
     string? keyword,
     int? categoryId,
     string? size,
     string? color,
     string? gender,
     bool sale = false,
     int page = 1)
        {
            int pageSize = 9;

            var products = _context.Products
                                   .Include(x => x.Category)
                                   .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(x => x.ProductName.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                products = products.Where(x => x.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(size))
            {
                products = products.Where(x => x.Size != null &&
                                               x.Size.Contains(size));
            }

            if (!string.IsNullOrEmpty(color))
            {
                products = products.Where(x => x.Color != null &&
                                               x.Color.Contains(color));
            }


            // Lọc Nam / Nữ
            if (!string.IsNullOrEmpty(gender))
            {
                products = products.Where(x => x.Gender == gender);
            }

            // Lọc SALE
            if (sale)
            {
                products = products.Where(x => x.SalePrice != null);
            }

            int totalProducts = products.Count();

            ViewBag.TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            ViewBag.CurrentPage = page;

            ViewBag.Categories = _context.Categories.ToList();

            var result = products.Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            return View(result);
        }

        //=============================
        // CHI TIẾT SẢN PHẨM
        //=============================
        public IActionResult Detail(int id)
        {
            var product = _context.Products
                                  .Include(p => p.Category)
                                  .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //=============================
        // THÊM SẢN PHẨM
        //=============================
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Categories = _context.Categories.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile imageFile)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() +
                                      Path.GetExtension(imageFile.FileName);

                    string uploadFolder = Path.Combine(_environment.WebRootPath, "images");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                    product.ImageUrl = "/images/" + fileName;
                }

                _context.Products.Add(product);
                _context.SaveChanges();

                TempData["Success"] = "Thêm sản phẩm thành công.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();

            return View(product);
        }

        //=============================
        // SỬA SẢN PHẨM
        //=============================
        //=============================
        // SỬA SẢN PHẨM (GET)
        //=============================
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Categories.ToList();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, IFormFile? imageFile)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            // Nếu ImageUrl không submit thì bỏ qua validate
            ModelState.Remove("ImageUrl");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            var oldProduct = _context.Products.Find(product.ProductId);

            if (oldProduct == null)
            {
                return NotFound();
            }

            oldProduct.ProductName = product.ProductName;
            oldProduct.Description = product.Description;
            oldProduct.Price = product.Price;
            oldProduct.SalePrice = product.SalePrice;
            oldProduct.Quantity = product.Quantity;
            oldProduct.Brand = product.Brand;
            oldProduct.Size = product.Size;
            oldProduct.Color = product.Color;
            oldProduct.CategoryId = product.CategoryId;
            oldProduct.Gender = product.Gender;

            // Chỉ cập nhật ảnh nếu có chọn ảnh mới
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(imageFile.FileName);

                string uploadFolder = Path.Combine(_environment.WebRootPath, "images");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                oldProduct.ImageUrl = "/images/" + fileName;
            }

            _context.SaveChanges();

            TempData["Success"] = "Cập nhật sản phẩm thành công.";

            return RedirectToAction(nameof(Index));
        }

        //=============================
        // XÓA SẢN PHẨM
        //=============================
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var product = _context.Products
                                  .Include(p => p.Category)
                                  .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var product = _context.Products.Find(id);

            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    string imagePath = Path.Combine(
                        _environment.WebRootPath,
                        product.ImageUrl.TrimStart('/').Replace("/", "\\"));

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Products.Remove(product);
                _context.SaveChanges();

                TempData["Success"] = "Xóa sản phẩm thành công.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}