using Microsoft.AspNetCore.Mvc;

public partial class CartController
{
    [HttpGet]
    [Route("Cart/ZaloPayCallBack")]
    public IActionResult ZaloPayCallBack()
    {
        var status = Request.Query["status"];
        if (status == "1")
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success", new { method = "ZaloPay" });
        }
        TempData["Error"] = "Transaction was unsuccessful";
        return RedirectToAction("Checkout");
    }

    [HttpGet]
    [Route("Checkout/PaymentCallBack")]
    public IActionResult PaymentCallBack()
    {
        var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
        string errorCode = HttpContext.Request.Query["errorCode"];

        if (errorCode == "0")
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success", new { method = "Momo" });
        }

        TempData["Error"] = "Transaction failed or canceled";
        return RedirectToAction("Checkout");
    }
}