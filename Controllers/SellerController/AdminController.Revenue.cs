using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public partial class AdminController
    {
        public IActionResult Revenue()
        {
            // Lấy 7 ngày gần đây để tránh tải quá nhiều dữ liệu cũ
            var startDate = DateTime.Now.AddDays(-6).Date;

            // CHỈ lấy đơn hàng đã hoàn thành
            var orders = _context.tb_Order
                .Where(o => o.OrderDate >= startDate && o.OrderStatus == "Completed")
                .ToList();

            ViewBag.TotalRevenue = orders.Sum(o => o.TotalPrice).ToString("N0");
            ViewBag.TotalOrders = orders.Count;

            // Group theo Date trước khi Format thành String để sắp xếp chuẩn
            var revenueByDate = orders
                .GroupBy(o => o.OrderDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new {
                    Date = g.Key.ToString("dd/MM"),
                    Total = g.Sum(x => x.TotalPrice)
                })
                .ToList();

            ViewBag.Dates = revenueByDate.Select(x => x.Date).ToList();
            ViewBag.Revenues = revenueByDate.Select(x => x.Total).ToList();

            // Top Products
            ViewBag.TopProducts = _context.tb_OrderDetails
                .Include(x => x.Product) // Phải Include để không bị lỗi Null Product
                .GroupBy(x => x.Product.ProductName)
                .Select(g => new { Name = g.Key, Quantity = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.Quantity)
                .Take(5).ToList();

            return View();
        }


    }
}