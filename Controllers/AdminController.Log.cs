using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public partial class AdminController
    {
        private void WriteLog(string fullSentence, string technicalDetails = "")
        {
            var userIdString = _userManager.GetUserId(User);
            if (userIdString == null) return;

            var log = new SystemLog
            {
                UserId = int.Parse(userIdString),
                Action = fullSentence, // Generate sentence: "Admin updated product A: Price from 10 to 20, Name from X to Y"
                Details = technicalDetails,
                Timestamp = DateTime.Now
            };
            _context.tb_SystemLog.Add(log);
        }

        // Tạo view để xem lịch sử Log
        public IActionResult Logs()
        {
            var logs = _context.tb_SystemLog.OrderByDescending(l => l.Timestamp).ToList();
            return View(logs);
        }
    }
}
