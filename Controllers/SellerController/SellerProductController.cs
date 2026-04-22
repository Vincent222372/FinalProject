using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.Controllers.SellerController
{
    [Authorize(Roles = "Shop")]
    public class SellerProductController : Controller
    {
        private readonly WebDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public SellerProductController(WebDbContext context, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        private int GetCurrentUserId()
        {
            var idStr = _userManager.GetUserId(User);
            return int.TryParse(idStr, out var id) ? id : 0;
        }

        private Shop GetSellerShop()
        {
            var userId = GetCurrentUserId();
            return _context.tb_Shop.FirstOrDefault(s => s.OwnerId == userId);
        }

        public IActionResult Index()
        {
            var shop = GetSellerShop();
            if (shop == null)
                return View(new List<Product>());

            // ===== PRODUCTS =====
            var products = _context.tb_Product
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.ShopId == shop.ShopId)
                .ToList();

            // ===== 🔥 ORDERS (THÊM MỚI) =====
            var orders = _context.tb_Order
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.OrderDetails.Any(od => od.Product.ShopId == shop.ShopId))
                .OrderByDescending(o => o.CreatedDate)
                .ToList();

            // ===== VIEWBAG =====
            ViewBag.Shop = shop;
            ViewBag.ShopOrders = orders;

            return View(products);
        }
        public IActionResult Details(int id)
        {
            var shop = GetSellerShop();
            var product = _context.tb_Product
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.ProductId == id && p.ShopId == shop.ShopId);

            if (product == null) return NotFound();
            return View(product);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            ViewBag.Brands = _context.tb_Brand.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model, IFormFile imageFile)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            if (!ModelState.IsValid)
            {
               
                // Upload ảnh chính
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads", "shops", shop.ShopId.ToString());
                    Directory.CreateDirectory(uploads);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using var fs = new FileStream(filePath, FileMode.Create);
                    imageFile.CopyTo(fs);

                    model.Image = Path.Combine("uploads", "shops", shop.ShopId.ToString(), fileName).Replace('\\', '/');
                    model.ListImages = "";
                }

                // 🔥 FIX LỖI NULL ListImages
                model.ListImages = model.ListImages ?? "";

                model.ShopId = shop.ShopId;
                model.CreatedBy = GetCurrentUserId();
                model.CreatedBy = GetCurrentUserId();
                model.CreatedDate = DateTime.Now;

                _context.tb_Product.Add(model);
                _context.SaveChanges();

                // cập nhật số lượng sản phẩm shop
                shop.TotalProducts = _context.tb_Product.Count(p => p.ShopId == shop.ShopId);
                _context.tb_Shop.Update(shop);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            ViewBag.Brands = _context.tb_Brand.ToList();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var shop = GetSellerShop();
            var product = _context.tb_Product.FirstOrDefault(p => p.ProductId == id && p.ShopId == shop.ShopId);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            ViewBag.Brands = _context.tb_Brand.ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model, IFormFile imageFile)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            var existing = _context.tb_Product.FirstOrDefault(p => p.ProductId == model.ProductId && p.ShopId == shop.ShopId);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads", "shops", shop.ShopId.ToString());
                    Directory.CreateDirectory(uploads);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using var fs = new FileStream(filePath, FileMode.Create);
                    imageFile.CopyTo(fs);

                    existing.Image = Path.Combine("uploads", "shops", shop.ShopId.ToString(), fileName).Replace('\\', '/');
                }

                existing.ProductName = model.ProductName;
                existing.SeoTitle = model.SeoTitle;
                existing.ProductDescription = model.ProductDescription;
                existing.Detail = model.Detail;
                existing.MetaKeywords = model.MetaKeywords;
                existing.MetaDescription = model.MetaDescription;
                existing.BrandId = model.BrandId;
                existing.CateId = model.CateId;
                existing.Status = model.Status;
                existing.Hot = model.Hot;
                existing.UpdatedBy = GetCurrentUserId();
                existing.UpdatedDate = DateTime.Now;

                _context.tb_Product.Update(existing);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            ViewBag.Brands = _context.tb_Brand.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var shop = GetSellerShop();
            var product = _context.tb_Product.FirstOrDefault(p => p.ProductId == id && p.ShopId == shop.ShopId);
            if (product == null) return NotFound();

            _context.tb_Product.Remove(product);
            _context.SaveChanges();

            shop.TotalProducts = _context.tb_Product.Count(p => p.ShopId == shop.ShopId);
            _context.tb_Shop.Update(shop);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}