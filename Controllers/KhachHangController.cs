using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MOBILUX.Data;
using MOBILUX.Helper;
using MOBILUX.ViewModels;

namespace MOBILUX.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly MobiluxContext db;

        public KhachHangController(MobiluxContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }
		[HttpPost]
		public IActionResult DangKy(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				var khachHang = new KhachHang
				{
					HoTenKh = model.HoTenKh,
					Sdt = model.Sdt,
					Email = model.Email,
					MatKhau = model.MatKhau,
					DiaChi = model.DiaChi,
					
				};

				db.KhachHangs.Add(khachHang);
				db.SaveChanges();

				// Có thể redirect về trang đăng nhập hoặc thông báo thành công
				return RedirectToAction("Index", "Home");
			}

			return View(model);
		}

        public IActionResult DonHangDaDat()
        {
            int maKh = int.Parse(User.FindFirst(MySetting.CLAIM_STAFFID).Value);



            var donHangs = db.DonHangs
                .Include(dh => dh.ChiTietHds)
                .ThenInclude(ct => ct.MaSpNavigation)
                .Where(dh => dh.MaKh == maKh)
                .OrderByDescending(dh => dh.NgayTao)
                .ToList();

            return View(donHangs);
        }

        [HttpPost]
        public IActionResult HuyDon(int id)
        {
            var donHang = db.DonHangs.Include(d => d.ChiTietHds).FirstOrDefault(d => d.MaDh == id);
            if (donHang == null)
            {
                return NotFound();
            }

            if (donHang.TrangThai != "Đã xác nhận")
            {
                donHang.TrangThai = "Đã hủy";
                db.SaveChanges();
            }

            return RedirectToAction("DonHangDaDat");
        }
    }
}
