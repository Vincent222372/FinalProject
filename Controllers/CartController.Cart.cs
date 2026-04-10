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
        if (product == null) return NotFound();

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

        if (!User.Identity.IsAuthenticated)
        {
            string returnUrl = actionType == "buyNow" ? "/Cart/Checkout" : "/Cart";
            return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
        }

        if (actionType == "buyNow") return RedirectToAction("Checkout", "Cart");
        return RedirectToAction("Index", "Cart");
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