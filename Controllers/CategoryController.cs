using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // Danh sách
        public IActionResult Index()
        {
            return View(_context.Categories.ToList());
        }

        // GET Create
        public IActionResult Create()
        {
            return View();
        }

        // POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();

                TempData["Success"] = "Thêm danh mục thành công!";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET Edit
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);

                _context.SaveChanges();

                TempData["Success"] = "Cập nhật thành công!";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET Delete
        public IActionResult Delete(int id)
        {
            var category = _context.Categories
                                   .FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories
                                   .Include(c => c.Products)
                                   .FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            if (category.Products.Any())
            {
                TempData["Error"] = "Danh mục đang có sản phẩm, không thể xóa.";

                return RedirectToAction(nameof(Index));
            }

            _context.Categories.Remove(category);

            _context.SaveChanges();

            TempData["Success"] = "Xóa thành công!";

            return RedirectToAction(nameof(Index));
        }
    }
}