using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOBILUX.Data;
using MOBILUX.Helper;
using System.Security.Claims;

namespace MOBILUX.Controllers
{
    [Route("customer/chat")]
    [Authorize(Roles = "customer")]

    public class CustomerChatController : Controller
    {
        private readonly MobiluxContext _context;

        public CustomerChatController(MobiluxContext context)
        {
            _context = context;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto dto)
        {
            var userIdStr = User.FindFirstValue("StaffId");
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var khachHang = await _context.KhachHangs.FindAsync(userId);
            if (khachHang == null) return NotFound();

            var lienHe = await _context.LienHes.FirstOrDefaultAsync(l => l.MaKh == userId);
            if (lienHe == null)
            {
                lienHe = new LienHe
                {
                    MaKh = userId,
                    Hoten = khachHang.HoTenKh,
                    NgayTao = DateTime.Now,
                    TrangThai = "Đang chat"
                };
                _context.LienHes.Add(lienHe);
                await _context.SaveChangesAsync();
            }

            var tinNhan = new TinNhanLienHe
            {
                MaLh = lienHe.MaLh,
                NguoiGui = "khachhang",
                NoiDung = dto.NoiDung,
                ThoiGianGui = DateTime.Now
            };

            _context.TinNhanLienHes.Add(tinNhan);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userIdStr = User.FindFirstValue(MySetting.CLAIM_STAFFID); // hoặc ClaimTypes.NameIdentifier nếu bạn dùng cái đó
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            var lienHe = await _context.LienHes
                .OrderByDescending(l => l.NgayTao)
                .FirstOrDefaultAsync(l => l.MaKh == userId);

            if (lienHe == null)
                return Ok(new List<object>()); // Không có tin nhắn nào → trả mảng rỗng

            var messages = await _context.TinNhanLienHes
                .Where(x => x.MaLh == lienHe.MaLh)
                .OrderBy(x => x.ThoiGianGui)
                .Select(x => new {
                    maLh = x.MaLh,
                    nguoiGui = x.NguoiGui,
                    noiDung = x.NoiDung,
                    thoiGianGui = x.ThoiGianGui
                })
                .ToListAsync();

            return Ok(messages);
        }


        [HttpGet("view")]
        public IActionResult ViewChat()
        {
            return View("~/Views/CustomerChat/Index.cshtml");
        }
    }

    public class ChatMessageDto
    {
        public string NoiDung { get; set; } = null!;
    }
}
