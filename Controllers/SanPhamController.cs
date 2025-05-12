using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOBILUX.Data;
using MOBILUX.ViewModels;

namespace MOBILUX.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly MobiluxContext db;

        public SanPhamController(MobiluxContext context)
        {
            db = context;
        }





        public IActionResult Index(int? loai, string searchTerm, int page = 1)
        {
            int pageSize = 9;
            var sanPhams = db.SanPhams
                .Include(p => p.MaDanhMucNavigation)
                .Where(p => p.TrangThai == "Còn hàng")
                .AsQueryable();

            if (loai.HasValue)
            {
                sanPhams = sanPhams.Where(p => p.MaDanhMuc == loai.Value);
            }

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
                    TenDanhMuc = p.MaDanhMucNavigation.TenDanhMuc,
                    
                })
                .ToList();

            ViewBag.TotalProducts = totalItems;
            ViewBag.SearchTerm = searchTerm;

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
                var danhGias = db.DanhGia
                    .Where(d => d.MaSp == id)
                    .OrderByDescending(d => d.NgayTao)
                    .ToList();

             ViewBag.DanhGias = danhGias;

            var result = new ChiTietSanPhamVM
            {
                MaSp = data.MaSp,
                TenSp = data.TenSp,
                DonGia = data.Gia ?? 0,
                Mota = data.MoTa ?? String.Empty,
                Hinh = data.Hinh ?? String.Empty,
                TenDanhMuc = data.MaDanhMucNavigation.TenDanhMuc,
                SoLuong = data.SoLuong ?? 0,
                GiamGia = data.GiamGia ?? 0,
                MotaNgan = data.MoTaNgan ?? String.Empty,
                TrangThai = data.TrangThai ?? String.Empty,
                Pin = data.Pin ?? String.Empty,
                Trongluong = data.TrongLuong ?? String.Empty,
                Dungluong = data.DungLuong ?? String.Empty,
                Spnb = data.MaSp + 1

            };
            return View(result);
        }
        //return View(result);

        [HttpPost]
        public IActionResult GuiDanhGia(DanhGiaVM model)
        {
            if (ModelState.IsValid)
            {
                var danhGia = new DanhGium
                {
                    MaSp = model.MaSp,
                    Hoten = model.HoTen,
                    Email = model.Email,
                    NoiDung = model.NoiDung,
                    NgayTao = DateTime.Now
                };
                db.DanhGia.Add(danhGia);
                db.SaveChanges();

                TempData["Success"] = "Gửi đánh giá thành công!";
                return RedirectToAction("Detail", new { id = model.MaSp });
            }

            TempData["Error"] = "Vui lòng điền đầy đủ thông tin.";
            return RedirectToAction("Detail", new { id = model.MaSp });
        }


    }
}

