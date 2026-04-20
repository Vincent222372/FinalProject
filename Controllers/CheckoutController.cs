using Azure;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Models.Momo;
using FinalProject.Services.Momo;
using Microsoft.AspNetCore.Mvc;

public class CheckoutController : Controller
{
    private readonly IMomoService _momoService;
    private readonly WebDbContext _context;

    public CheckoutController(IMomoService momoService, WebDbContext context)
    {
        _momoService = momoService;
        _context = context;
    }

    // 👉 BẤM THANH TOÁN
    [HttpPost]
    public async Task<IActionResult> Checkout(string paymentMethod, string PromotionCode)
    {

        decimal originalTotal = 55000; // You should calculate this from Session/DB cart items
        decimal finalTotal = originalTotal;
        int discountPercent = 0;

        if (!string.IsNullOrEmpty(PromotionCode))
        {
            var now = DateTime.Now;
            var promo = _context.tb_Promotion.FirstOrDefault(p =>
                p.PromotionCode == PromotionCode &&
                p.Status == true &&
                now >= p.StartDate &&
                now <= p.EndDate);

            if (promo != null)
            {
                discountPercent = promo.DiscountPercent;
                finalTotal = originalTotal * (100 - discountPercent) / 100;
            }
        }
        // 1. Tạo đơn hàng
        var order = new Order
        {
            UserId = 1, // Should get from User.Identity
            OrderStatus = "Pending",
            PaymentStatus = "Unpaid",
            OrderDate = DateTime.Now,
            TotalPrice = finalTotal, // Store the final price!
            ShippingAddress = "Fixed Address for test", // You have FullName/Address in VM
            ReceiverPhone = "0123456789"
        };

        _context.tb_Order.Add(order);
        await _context.SaveChangesAsync();

        // 2. COD → xong luôn
        if (paymentMethod == "COD")
        {
            order.OrderStatus = "Completed";
            order.PaymentStatus = "Paid";
            await _context.SaveChangesAsync();

            return RedirectToAction("Result", "Checkout", new { id = order.OrderId });
        }

        // 3. MOMO
        var momoModel = new OrderInfoModel
        {
            FullName = "Nguyen Thanh Dat",
            Amount = (double)finalTotal,

            // 🔥 FIX Ở ĐÂY
            OrderInfo = "Order Payment " + order.OrderId + (discountPercent > 0 ? $" (Discount {discountPercent}%)" : "")
        };
        var response = await _momoService.CreatePaymentMomo(momoModel);

        if (response != null && !string.IsNullOrEmpty(response.PayUrl))
        {
            // Đây là lệnh quan trọng để mở trang quét mã QR MoMo
            return Redirect(response.PayUrl);
        }

        // 👉 in ra để xem lỗi thật
        return Content(
            "Message: " + response.Message +
            "\nPayUrl: " + response.PayUrl
        );
    }

    // 👉 MOMO TRẢ VỀ
    public async Task<IActionResult> MomoReturn()
    {
        try
        {
            var query = HttpContext.Request.Query;

            var orderInfo = query["orderInfo"].ToString();
            var resultCode = query["resultCode"].ToString();

            var orderIdFromMomo = query["orderId"].ToString();
            // 🔥 FIX: không parse bừa nữa
            if (int.TryParse(orderIdFromMomo, out int orderId))
            {
                var order = _context.tb_Order.FirstOrDefault(o => o.OrderId == orderId);

                if (order != null)
                {
                    if (resultCode == "0")
                    {
                        order.OrderStatus = "Completed";
                        order.PaymentStatus = "Paid";
                        ViewBag.Message = "Transaction Successful";
                    }
                    else
                    {
                        order.OrderStatus = "Cancelled";
                        order.PaymentStatus = "Failed";
                        ViewBag.Message = "Transaction Failed";
                    }

                    await _context.SaveChangesAsync();
                    return View("PaymentSuccess", order);
                }
            }

            return View("PaymentSuccess");
        }
        catch (Exception ex)
        {
            return Content("ERROR: " + ex.Message);
        }
    }
    public IActionResult Result(int id)
    {
        var order = _context.tb_Order.FirstOrDefault(o => o.OrderId == id);
        return View("PaymentSuccess", order);
    }
}