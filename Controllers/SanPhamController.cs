using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOBILUX.Data;
using MOBILUX.ViewModels;

namespace MOBILUX.Controllers
{
	public class SanPhamController : Controller
	{
		private readonly MobiluxContext db;

		public SanPhamController(MobiluxContext context) {
			db = context;
		}



		//public IActionResult Index(int? loai, int page = 1)
		//{
		//	int pageSize = 9;
		//	var sanPhams = db.SanPhams.AsQueryable();

		//	if (loai.HasValue)
		//	{
		//		sanPhams = sanPhams.Where(p => p.MaDanhMuc == loai.Value);
		//	}

		//	int totalItems = sanPhams.Count();

		//	//var result = sanPhams.Select(p => new SanPhamVM
		//	var paginatedHangHoas = sanPhams
		//		.Skip((page - 1) * pageSize)
		//		.Take(pageSize)
		//		.Select(p => new SanPhamVM
		//		{
		//		MaSp = p.MaSp,
		//		TenSp = p.TenSp,
		//		DonGia = p.Gia ?? 0,
		//		Hinh = p.Hinh ?? "",
		//		MotaNgan = p.MoTaNgan ?? "",
		//		TenDanhMuc = p.MaDanhMucNavigation.TenDanhMuc
		//	})
		//	.ToList();

		//	ViewBag.TotalProducts = sanPhams.Count();

		//	var model = new PaginatedSanPhamVM
		//	{
		//		SanPhams = paginatedHangHoas,
		//		CurrentPage = page,
		//		TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
		//	};

		//	return View(model);
		public IActionResult Index(int? loai, string searchTerm, int page = 1)
		{
			int pageSize = 9;
			var sanPhams = db.SanPhams.AsQueryable();

			// Lọc theo danh mục (nếu có)
			if (loai.HasValue)
			{
				sanPhams = sanPhams.Where(p => p.MaDanhMuc == loai.Value);
			}

			// Lọc theo từ khóa tìm kiếm (nếu có)
			if (!string.IsNullOrEmpty(searchTerm))
			{
				sanPhams = sanPhams.Where(p => p.TenSp.Contains(searchTerm));
			}

			int totalItems = sanPhams.Count();

			var paginatedHangHoas = sanPhams
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(p => new SanPhamVM
				{
					MaSp = p.MaSp,
					TenSp = p.TenSp,
					DonGia = p.Gia ?? 0,
					Hinh = p.Hinh ?? "",
					MotaNgan = p.MoTaNgan ?? "",
					TenDanhMuc = p.MaDanhMucNavigation.TenDanhMuc
				})
				.ToList();

			ViewBag.TotalProducts = sanPhams.Count();
			ViewBag.SearchTerm = searchTerm; // Giữ lại giá trị ô tìm kiếm

			var model = new PaginatedSanPhamVM
			{
				SanPhams = paginatedHangHoas,
				CurrentPage = page,
				TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
			};

			return View(model);
		}

		public IActionResult Detail(int id)
		{
			var data = db.SanPhams
				.Include(p => p.MaDanhMucNavigation)
				.SingleOrDefault(p => p.MaSp == id);
			if (data == null)
			{
				TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
				return Redirect("/404");
			}
			var result = new ChiTietSanPhamVM
			{
				MaSp = data.MaSp,
				TenSp = data.TenSp,
				DonGia = data.Gia ?? 0,
				Mota = data.MoTa ?? String.Empty,
				Hinh = data.Hinh ?? String.Empty,
				TenDanhMuc = data.MaDanhMucNavigation.TenDanhMuc,
				SoLuong = data.SoLuong ??0,
				GiamGia = data.GiamGia ??0,
				MotaNgan = data.MoTaNgan ?? String.Empty,
				TrangThai = data.TrangThai ??String.Empty,
				Pin = data.Pin ?? String.Empty,
				Trongluong = data.TrongLuong ?? String.Empty,
				Dungluong =data.DungLuong ?? String.Empty,
				Spnb = data.MaSp +1 

			};
			return View(result);
		}
		//return View(result);



	}
}

