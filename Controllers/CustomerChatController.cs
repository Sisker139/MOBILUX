using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOBILUX.Data;
using System.Security.Claims;

namespace MOBILUX.Controllers
{
    public class CustomerChatController : Controller
    {
        
            private readonly MobiluxContext _context;

            public CustomerChatController(MobiluxContext context)
            {
                _context = context;
            }

            public class SendMessageDto
            {
                public string MessageText { get; set; }
            }

            [HttpPost("send")]
            public async Task<IActionResult> SendMessage([FromBody] SendMessageDto request)
            {
                if (request == null || string.IsNullOrWhiteSpace(request.MessageText))
                    return BadRequest(new { error = "Tin nhắn không hợp lệ." });

                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                    return Unauthorized(new { error = "Bạn cần đăng nhập để gửi tin nhắn." });

                if (!int.TryParse(userIdStr, out int userId))
                    return BadRequest(new { error = "ID người dùng không hợp lệ." });

                var khachHang = await _context.KhachHangs.FindAsync(userId);
                if (khachHang == null)
                    return NotFound(new { error = "Không tìm thấy khách hàng." });

                var lienHe = new LienHe
                {
                    MaKh = userId,
                    Hoten = khachHang.HoTenKh,
                    NoiDung = request.MessageText,
                    NgayTao = DateTime.UtcNow,
                    TrangThai = "Chưa trả lời"
                };

                await _context.LienHes.AddAsync(lienHe);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }

            [HttpGet("get")]
            public async Task<IActionResult> GetUserMessages()
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                    return Unauthorized(new { error = "Bạn cần đăng nhập." });

                if (!int.TryParse(userIdStr, out int userId))
                    return BadRequest(new { error = "ID người dùng không hợp lệ." });

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

                return Ok(messages);
            
            }
    }
}
