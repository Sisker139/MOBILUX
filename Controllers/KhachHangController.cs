using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MOBILUX.Data;
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

	}
}
