using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

public partial class CartController
{
    public IActionResult Index()
    {
        var cart = GetCart();
        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddToCart(int productId, int quantity = 1, string actionType = "add")
    {
        var product = _context.tb_Product.FirstOrDefault(p => p.ProductID == productId);
        if (product == null) return Json(new { success = false, message = "Không tìm thấy sản phẩm" });

        var cart = GetCart();
        var existingItem = cart.FirstOrDefault(x => x.ProductID == productId);

        if (existingItem == null)
        {
            cart.Add(new CartItems
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Image = product.Image,
                Price = product.Price,
                Quantity = quantity
            });
        }
        else
        {
            existingItem.Quantity += quantity;
        }

        SaveCart(cart);

        // 1. Kiểm tra đăng nhập trước
        if (!User.Identity.IsAuthenticated)
        {
            string loginUrl = Url.Action("Login", "Account", new
            {
                returnUrl = actionType == "buyNow" ? "/Cart/Checkout" : "/Cart"
            });

            return Json(new { success = true, redirectUrl = loginUrl });
        }

        // 2. Nếu đã đăng nhập, trả về URL tương ứng với hành động
        return Json(new
        {
            success = true,
            redirectUrl = actionType == "buyNow" ? "/Cart/Checkout" : null
        });
    }

    public IActionResult Remove(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.ProductID == id);
        if (item != null) cart.Remove(item);
        SaveCart(cart);
        return RedirectToAction("Index");
    }
}