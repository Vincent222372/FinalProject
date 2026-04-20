using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public partial class AccountController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> PostReview(int productId, int rating, string comment)
        {
            // 1. Kiểm tra đăng nhập
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "Bạn cần đăng nhập để đánh giá!" });

            // 2. Lưu Review mới
            var review = new ProductReview
            {
                ProductId = productId,
                UserId = user.Id,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _context.tb_ProductReview.Add(review);
            await _context.SaveChangesAsync();

            // 3. TÍNH TOÁN LẠI SỐ SAO TRUNG BÌNH (Cập nhật vào cột Star của bảng Product)
            var allReviews = _context.tb_ProductReview.Where(r => r.ProductId == productId);
            decimal averageStar = (decimal)allReviews.Average(r => r.Rating);

            var product = await _context.tb_Product.FindAsync(productId);
            if (product != null)
            {
                product.Star = averageStar; // Cập nhật cột Star dư thừa để load danh sách cho nhanh
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Cảm ơn bạn đã đánh giá!", averageStar });
        }
    }
}
