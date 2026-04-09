using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FinalProject.ViewModels;

namespace FinalProject.Controllers
{
    public partial class AccountController
    {
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid) return View(model);

            
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
  
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("Shop"))
                    {
                        return RedirectToAction("Index", "SellerShop");
                    }
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account is temporarily locked. Please try again later.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}