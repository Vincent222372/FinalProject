using FinalProject.Models;
using FinalProject.ViewModels;
using FinalProject.Services.Momo;
using Microsoft.AspNetCore.Mvc;

public partial class CartController
{
    [HttpGet]
    public IActionResult Checkout()
    {
        var cart = GetCart();
        if (cart.Count == 0) return RedirectToAction("Index", "Product");

        ViewBag.Cart = cart;
        ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutVM model)
    {
        var cart = GetCart();
        double totalAmount = (double)cart.Sum(x => (double)x.Price * x.Quantity);

        if (!ModelState.IsValid)
        {
            ViewBag.Cart = cart;
            ViewBag.Total = totalAmount;
            return View(model);
        }

        if (model.PaymentMethod == "Momo")
        {
            var orderInfo = new OrderInfoModel { FullName = model.FullName, Amount = totalAmount, OrderInfo = "Thanh toán đơn hàng Fashion Store" };
            var response = await _momoService.CreatePaymentMomo(orderInfo);
            if (response != null && !string.IsNullOrEmpty(response.PayUrl)) return Redirect(response.PayUrl);

            ModelState.AddModelError("", "Hệ thống Momo đang bận.");
        }

        if (model.PaymentMethod == "ZaloPay")
        {
            var orderInfo = new OrderInfoModel { FullName = model.FullName, Amount = totalAmount, OrderInfo = "Thanh toán đơn hàng Fashion Store" };
            var payUrl = await _zaloPayService.CreatePaymentUrl(orderInfo);
            if (!string.IsNullOrEmpty(payUrl)) return Redirect(payUrl);

            ModelState.AddModelError("", "Không thể khởi tạo thanh toán ZaloPay.");
        }

        if (model.PaymentMethod == "COD")
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success", new { method = "COD" });
        }

        ViewBag.Cart = cart;
        return View(model);
    }
}