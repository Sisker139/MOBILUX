using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOBILUX.Data;

namespace MOBILUX.Controllers
{
	[Route("admin/chat")]
    [Authorize(Roles = "Admin,staff")]
    public class AdminChatController : Controller
	{
		private readonly MobiluxContext _context;

		public AdminChatController(MobiluxContext context)
		{
			_context = context;
		}

        [HttpGet("contacts")]
        public async Task<IActionResult> GetContacts()
        {
            var data = await _context.TinNhanLienHes
                .GroupBy(m => m.MaLh)
                .Select(g => new
                {
                    MaLh = g.Key,
                    // ✅ luôn lấy tên từ bảng LienHe
                    TenKhach = g.Select(x => x.LienHe.Hoten).FirstOrDefault(),
                    ThoiGianCuoi = g.Max(x => x.ThoiGianGui)
                })
                .OrderByDescending(x => x.ThoiGianCuoi)
                .ToListAsync();

            return Json(data);
        }


        [HttpGet("history/{maLh}")]
		public async Task<IActionResult> GetHistory(int maLh)
		{
			var messages = await _context.TinNhanLienHes
				.Where(m => m.MaLh == maLh)
				.OrderBy(m => m.ThoiGianGui)
				.ToListAsync();

			return Ok(messages);
		}

		[HttpPost("send")]
		public async Task<IActionResult> SendMessage([FromBody] AdminMessageDto dto)
		{
			var tinNhan = new TinNhanLienHe
			{
				MaLh = dto.MaLh,
				NguoiGui = "nhanvien",
				NoiDung = dto.NoiDung,
				ThoiGianGui = DateTime.Now
			};

			_context.TinNhanLienHes.Add(tinNhan);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpGet("view/{maLh}")]
		public IActionResult ViewChat(int maLh)
		{
			ViewBag.MaLh = maLh;
			return View("~/Views/AdminChat/Index.cshtml");
		}
	}

	public class AdminMessageDto
	{
		public int MaLh { get; set; }
		public string NoiDung { get; set; } = null!;
	}
}
