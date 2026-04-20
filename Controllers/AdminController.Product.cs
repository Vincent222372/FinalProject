using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public partial class AdminController
    {
        public IActionResult Products()
        {
            return View(_context.tb_Product
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Shop)
                .ToList());
        }

        public IActionResult ProductDetail(int id)
        {
            var product = _context.tb_Product
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Shop)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null) return NotFound();

            return PartialView("_ProductDetailPartial", product);
        }

        public IActionResult EditProduct(int id)
        {
            var product = _context.tb_Product.Find(id);
            if (product == null) return NotFound();
            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product model)
        {
            var oldProduct = _context.tb_Product.AsNoTracking().FirstOrDefault(p => p.ProductId == model.ProductId);
            if (oldProduct == null) return NotFound();

            var changes = new List<string>();

            // Tự động lấy tất cả các thuộc tính (Properties) của Product để so sánh
            foreach (var prop in typeof(Product).GetProperties())
            {
                var oldValue = prop.GetValue(oldProduct);
                var newValue = prop.GetValue(model);

                // Nếu giá trị khác nhau và không phải là mấy cái ID hay Navigation Property thì log lại
                if (newValue != null && !newValue.Equals(oldValue) && !prop.Name.Contains("Id") && !prop.Name.Contains("Category"))
                {
                    changes.Add($"{prop.Name} changed from '{oldValue}' to '{newValue}'");
                }
            }

            string adminName = _context.tb_Users.Find(int.Parse(_userManager.GetUserId(User)))?.FullName ?? "Admin";
            string logMsg = $"{adminName} updated {model.ProductName}: " + (changes.Any() ? string.Join(", ", changes) : "no changes");

            WriteLog(logMsg);
            _context.tb_Product.Update(model);
            _context.SaveChanges();

            return RedirectToAction("Products");
        }

        public IActionResult Categories() => View(_context.tb_ProductCategory.ToList());

        public IActionResult CreateCategory() => View();

        [HttpPost]
        public IActionResult CreateCategory(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                _context.tb_ProductCategory.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View(model);
        }

        public IActionResult DeleteCategory(int id)
        {
            var cate = _context.tb_ProductCategory.Find(id);
            if (cate != null)
            {
                _context.tb_ProductCategory.Remove(cate);
                _context.SaveChanges();
            }
            return RedirectToAction("Categories");
        }
    }
}