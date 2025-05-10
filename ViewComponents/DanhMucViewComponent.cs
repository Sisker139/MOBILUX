using Microsoft.AspNetCore.Mvc;
using MOBILUX.Data;
using MOBILUX.ViewModels;
using System.Linq;

namespace MOBILUX.ViewComponents
{
	public class DanhMucViewComponent : ViewComponent
	{
		private readonly MobiluxContext db;

		public DanhMucViewComponent(MobiluxContext context) => db = context;

		public IViewComponentResult Invoke()
		{
			var data = db.DanhMucs
				.Where(dm => dm.TrangThai == "Hoạt động")  // ✅ Lọc trạng thái
				.Select(dm => new DanhMucVM
				{
					MaDanhMuc = dm.MaDanhMuc,
					TenDanhMuc = dm.TenDanhMuc,
					SoLuong = dm.SanPhams.Count,
					TrangThai = dm.TrangThai
				})
				.ToList();

			return View(data);
		}
	}
}
