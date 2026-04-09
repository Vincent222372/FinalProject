using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

public class CartController : Controller
{
    private readonly WebDbContext _context;

    public CartController(WebDbContext context)
    {
        _context = context;
    }

    // VIEW CART
    public IActionResult Index()
    {
        var cart = GetCart();
        return View(cart);
    }

    // ADD TO CART
    [HttpPost] // Nên dùng HttpPost để bảo mật dữ liệu giỏ hàng
    public IActionResult AddToCart(int productId, int quantity = 1, string actionType = "add")
    {
        var product = _context.tb_Product.FirstOrDefault(p => p.ProductID == productId);
        if (product == null) return NotFound();

        var cart = GetCart();
        var existingItem = cart.FirstOrDefault(x => x.ProductID == productId);

        // 1. Logic thêm/cập nhật giỏ hàng phải chạy TRƯỚC
        if (existingItem == null)
        {
            cart.Add(new CartItems
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Image = product.Image,
                Price = product.Price,
                Quantity = quantity // Dùng tham số quantity truyền vào
            });
        }
        else
        {
            existingItem.Quantity += quantity;
        }

        // 2. Lưu giỏ hàng mới vào Session/Cookie
        SaveCart(cart);

        // 3. Kiểm tra xem người dùng nhấn nút nào để điều hướng
        if (!User.Identity.IsAuthenticated)
        {
            // Nếu chưa đăng nhập, dù bấm nút nào cũng phải qua trang Login
            // Lưu ý: returnUrl nên trỏ về trang thanh toán nếu là Buy Now, hoặc trang giỏ hàng nếu là Add
            string returnUrl = actionType == "buyNow" ? "/Cart/Checkout" : "/Cart";
            return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
        }

        // 4. Nếu đã đăng nhập, phân loại điều hướng theo actionType
        if (actionType == "buyNow")
        {
            return RedirectToAction("Checkout", "Cart");
        }

        // Mặc định cho Add to Cart là về trang giỏ hàng
        return RedirectToAction("Index", "Cart");
    }

    // REMOVE
    public IActionResult Remove(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.ProductID == id);

        if (item != null)
        {
            cart.Remove(item);
        }

        SaveCart(cart);

        return RedirectToAction("Index");
    }

    // GET SESSION
    private List<CartItems> GetCart()
    {
        var session = HttpContext.Session.GetString("Cart");
        return session != null
            ? JsonConvert.DeserializeObject<List<CartItems>>(session)
            : new List<CartItems>();
    }

    // SAVE SESSION
    private void SaveCart(List<CartItems> cart)
    {
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
    }
}