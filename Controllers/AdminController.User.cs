using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public partial class AdminController
    {
        public IActionResult Users() => View(_context.tb_Users.ToList());

        public IActionResult LockUser(int id)
        {
            var user = _context.tb_Users.Find(id);
            if (user == null) return NotFound();
            user.IsActive = false;
            user.IsBanned = true;
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public IActionResult UnlockUser(int id)
        {
            var user = _context.tb_Users.Find(id);
            if (user == null) return NotFound();
            user.IsActive = true;
            user.IsBanned = false;
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        
    }
}