using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOBILUX.Data;

namespace MOBILUX.Controllers
{
    public class AdminChatController : Controller
    {
        private readonly MobiluxContext _context;

        public AdminChatController(MobiluxContext context)
        {
            _context = context;
        }

        [HttpGet]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserContacts()
        {
            var users = await _context.KhachHangs
                .Where(kh => _context.LienHes.Any(lh => lh.MaKh == kh.MaKh))
                .Select(kh => new
                {
                    id = kh.MaKh,
                    name = kh.HoTenKh ?? "Ẩn danh"
                })
                .ToListAsync();

            return Json(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(int userId)
        {
            var messages = await _context.LienHes
                .Where(m => m.MaKh == userId)
                .OrderBy(m => m.NgayTao)
                .Select(m => new
                {
                    id = m.MaLh,
                    messageText = m.NoiDung,
                    timestamp = m.NgayTao,
                    status = m.TrangThai
                })
                .ToListAsync();

            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int messageId, string newStatus)
        {
            var message = await _context.LienHes.FindAsync(messageId);
            if (message == null)
                return NotFound();

            message.TrangThai = newStatus;
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }
}
