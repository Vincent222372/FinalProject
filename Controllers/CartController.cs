using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Services.Momo;
using FinalProject.Services.Zalo;
using FinalProject.Services.ZaloPay;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public partial class CartController : Controller
{
    private readonly WebDbContext _context;
    private readonly IMomoService _momoService;
    private readonly IZaloPayService _zaloPayService;

    public CartController(WebDbContext context, IMomoService momoService, IZaloPayService zaloPayService)
    {
        _context = context;
        _momoService = momoService;
        _zaloPayService = zaloPayService;
    }

    private List<CartItems> GetCart()
    {
        var session = HttpContext.Session.GetString("Cart");
        if (string.IsNullOrEmpty(session)) return new List<CartItems>();
        return JsonConvert.DeserializeObject<List<CartItems>>(session);
    }

    private void SaveCart(List<CartItems> cart)
    {
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
    }

    public IActionResult Success(string method)
    {
        ViewBag.Method = method;
        return View();
    }
}