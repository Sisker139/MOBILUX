using Microsoft.AspNetCore.Mvc;
using MOBILUX.Data;
using MOBILUX.ViewModels;

namespace MOBILUX.ViewComponents
{
	public class DanhMucViewComponent : ViewComponent
	{
		private readonly MobiluxContext db;

		public DanhMucViewComponent(MobiluxContext context) => db = context;

		public IViewComponentResult Invoke()
		{
			var data = db.DanhMucs.Select(dm => new DanhMucVM
			{
				MaDanhMuc = dm.MaDanhMuc,
				TenDanhMuc = dm.TenDanhMuc,
				SoLuong = dm.SanPhams.Count
			});
			return View(data);
		}
	}
}
