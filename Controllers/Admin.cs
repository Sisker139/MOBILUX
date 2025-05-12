using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOBILUX.Data;
using MOBILUX.ViewModels;
using MOBILUX.Helper;
using Microsoft.AspNetCore.Authorization;

namespace MOBILUX.Controllers
{
    [Authorize(Roles = "Admin,staff")]
    public class Admin : Controller
    {

        private readonly MobiluxContext db;

        public Admin(MobiluxContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            var products = db.SanPhams.Include(p => p.MaDanhMucNavigation).ToList();
            return View(products);
        }



        public IActionResult Create()
        {
            var loaiSanPhams = db.DanhMucs.ToList();
            if (loaiSanPhams == null || !loaiSanPhams.Any())
            {
                // Gán danh sách rỗng và có thể log thông báo
                loaiSanPhams = new List<DanhMuc>();
                Console.WriteLine("Danh sách loại sản phẩm bị null hoặc không có dữ liệu.");
            }

            var model = new ThemSanPhamVM
            {
                ListDM = loaiSanPhams
            };


            return View(model);
        }
        [HttpPost]

        public async Task<IActionResult> Create(ThemSanPhamVM model, IFormFile Hinh)
        {
            var loaiSanPhams = db.DanhMucs?.ToList();
            Console.WriteLine(loaiSanPhams?.Count ?? 0); // In ra số lượng phần tử

            try
            {
                // Mapping từ model sang entity

                var hangHoa = new SanPham
                {
                    
                    TenSp = model.TenSP,
                    MaDanhMuc = model.MaDanhMuc,
                    MoTa = model.MoTa,
                    MoTaNgan = model.MoTaNgan,
                    Gia = model.Gia,
                    GiamGia = model.GiamGia,
                    SoLuong = model.SoLuong,
                    Pin = model.Pin,
                    TrongLuong = model.TrongLuong,
                    DungLuong = model.DungLuong,
                    TrangThai = model.TrangThai,
                };


                // Xử lý hình ảnh nếu có
                if (Hinh != null)
                {
                    var hinhPath = MyUtil.UploadHinh(Hinh, "Phone");
                    if (!string.IsNullOrEmpty(hinhPath))
                    {
                        hangHoa.Hinh = hinhPath;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể upload hình ảnh");
                        return View(model);
                    }
                }

                // Thêm vào DbContext và lưu thay đổi
                db.SanPhams.Add(hangHoa);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                // Ghi log hoặc hiển thị lỗi
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
            }


            // Nếu ModelState không hợp lệ hoặc lỗi xảy ra
            return View(model);
        }


        public IActionResult Details(int id)
        {
            var sp = db.SanPhams
                .Include(p => p.MaDanhMucNavigation)
                .FirstOrDefault(p => p.MaSp == id);

            if (sp == null)
            {
                return NotFound();
            }

            return View(sp);
        }






        [HttpGet]
        public IActionResult Edit(int id)
        {
            var sp = db.SanPhams.FirstOrDefault(x => x.MaSp == id);
            if (sp == null)
            {
                return NotFound();
            }

            var model = new ThemSanPhamVM
            {
                TenSP = sp.TenSp,
                MaDanhMuc = sp.MaDanhMuc,
                MoTa = sp.MoTa,
                MoTaNgan = sp.MoTaNgan,
                Gia = sp.Gia,
                GiamGia = sp.GiamGia ?? 0,
                SoLuong = sp.SoLuong ?? 0,
                Hinh = sp.Hinh,
                DungLuong = sp.DungLuong,
                TrongLuong = sp.TrongLuong,
                Pin = sp.Pin,
                TrangThai = sp.TrangThai,
                ListDM = db.DanhMucs.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ThemSanPhamVM model, IFormFile HinhMoi)
        {
            var sp = db.SanPhams.FirstOrDefault(x => x.MaSp == id);
            if (sp == null)
            {
                return NotFound();
            }
            ModelState.Remove("HinhMoi");
            if (ModelState.IsValid)
            {
                sp.TenSp = model.TenSP;
                sp.MaDanhMuc = model.MaDanhMuc;
                sp.MoTa = model.MoTa;
                sp.MoTaNgan = model.MoTaNgan;
                sp.Gia = model.Gia;
                sp.GiamGia = model.GiamGia;
                sp.SoLuong = model.SoLuong;
                sp.DungLuong = model.DungLuong;
                sp.TrongLuong = model.TrongLuong;
                sp.Pin = model.Pin;
                sp.TrangThai = model.TrangThai;

                if (HinhMoi != null)
                {
                    var hinhPath = MyUtil.UploadHinh(HinhMoi, "Phone");
                    if (!string.IsNullOrEmpty(hinhPath))
                    {
                        sp.Hinh = hinhPath;
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.ListDM = db.DanhMucs.ToList();
            return View(model);
        }




        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var sp = db.SanPhams.FirstOrDefault(x => x.MaSp == id);
            if (sp == null)
            {
                return NotFound();
            }

            if (sp.TrangThai == "Còn hàng")
            {
                sp.TrangThai = "Hết hàng";
            }
            else
            {
                sp.TrangThai = "Còn hàng";
            }

            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult DanhMuc()
        {
            var danhMucs = db.DanhMucs
                .Select(d => new DanhMucVM
                {
                    MaDanhMuc = d.MaDanhMuc,
                    TenDanhMuc = d.TenDanhMuc,
                    SoLuong = d.SanPhams.Count,
                    TrangThai = d.TrangThai 
                })
                .ToList();

            return View(danhMucs);
        }

        [HttpGet]
        public IActionResult ThemDanhMuc()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ThemDanhMuc(DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                db.DanhMucs.Add(danhMuc);
                db.SaveChanges();
                return RedirectToAction("DanhMuc");
            }
            return RedirectToAction("ThemDanhMuc"); 
        }






        [HttpGet]
        public IActionResult EditDanhMuc(int id)
        {
            var danhMuc = db.DanhMucs.FirstOrDefault(d => d.MaDanhMuc == id);
            if (danhMuc == null)
            {
                return NotFound();
            }
            return View(danhMuc);
        }

        [HttpPost]
        public IActionResult EditDanhMuc(DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                var existing = db.DanhMucs.FirstOrDefault(d => d.MaDanhMuc == danhMuc.MaDanhMuc);
                if (existing != null)
                {
                    existing.TenDanhMuc = danhMuc.TenDanhMuc;
                    existing.TrangThai = danhMuc.TrangThai;
                    db.SaveChanges();
                    return RedirectToAction("DanhMuc");
                }
                return NotFound();
            }
            return View(danhMuc);
        }


        [HttpPost]
        public IActionResult ToggleDanhMucStatus(int id)
        {
            var danhMuc = db.DanhMucs.FirstOrDefault(d => d.MaDanhMuc == id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            if (danhMuc.TrangThai == "Hoạt động")
            {
                danhMuc.TrangThai = "Không hoạt động";
            }
            else
            {
                danhMuc.TrangThai = "Hoạt động";
            }

            db.SaveChanges();
            return RedirectToAction("DanhMuc");
        }


        public IActionResult QuanLyDH()
        {
            var donHangs = db.DonHangs
                .Include(dh => dh.MaKhNavigation)
                .Include(dh => dh.ChiTietHds)
                .ThenInclude(ct => ct.MaSpNavigation)
                .OrderByDescending(dh => dh.NgayTao)  // ✅ Sắp xếp mới nhất → cũ nhất
                .ToList();

            return View(donHangs);
        }

        [HttpPost]
        public IActionResult XacNhanDH(int id)
        {
            var donHang = db.DonHangs.FirstOrDefault(dh => dh.MaDh == id);
            if (donHang == null)
            {
                return NotFound();
            }

            if (donHang.TrangThai != "Đã xác nhận")
            {
                donHang.TrangThai = "Đã xác nhận";
                db.SaveChanges();
            }

            return RedirectToAction("QuanLyDH");
        }

        [HttpPost]
        public IActionResult HuyDH(int id)
        {
            var donHang = db.DonHangs.FirstOrDefault(dh => dh.MaDh == id);
            if (donHang == null)
            {
                return NotFound();
            }

            if (donHang.TrangThai != "Đã xác nhận" && donHang.TrangThai != "Đã hủy")
            {
                donHang.TrangThai = "Đã hủy";
                db.SaveChanges();
            }

            return RedirectToAction("QuanLyDH");
        }

        public async Task<IActionResult> QuanLyKhachHang()
        {
            var khachHangs = await db.KhachHangs.ToListAsync();
            return View(khachHangs);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var khachHang = await db.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }

            khachHang.MatKhau = "abcxyz"; // hoặc hash mật khẩu nếu đang mã hóa
            await db.SaveChangesAsync();

            TempData["Message"] = $"Mật khẩu của khách hàng '{khachHang.HoTenKh}' đã được đặt lại.";
            return RedirectToAction(nameof(QuanLyKhachHang));
        }

        public async Task<IActionResult> QuanLyNhanVien()
        {
            var nhanViens = await db.NhanViens.ToListAsync();
            return View(nhanViens);
        }



        [HttpPost]
        public async Task<IActionResult> ResetMatKhau(int id)
        {
            var nv = await db.NhanViens.FindAsync(id);
            if (nv == null)
            {
                return NotFound();
            }

            nv.MatKhau = "abcxyz"; // Bạn nên hash mật khẩu nếu hệ thống đang sử dụng bảo mật
            await db.SaveChangesAsync();

            TempData["Message"] = $"Mật khẩu của nhân viên {nv.TenNv} đã được đặt lại.";
            return RedirectToAction(nameof(QuanLyNhanVien));
        }







        
        public IActionResult ThongTin()
        {
            int manv = int.Parse(User.FindFirst(MySetting.CLAIM_STAFFID).Value);

            var nv = db.NhanViens.FirstOrDefault(k => k.MaNv == manv);
            if (nv == null)
                return NotFound();

            return View(nv);
        }

        
        [HttpPost]
        public IActionResult CapNhatThongTin(NhanVien model)
        {
            int manv = int.Parse(User.FindFirst(MySetting.CLAIM_STAFFID).Value);
            var nv = db.NhanViens.FirstOrDefault(k => k.MaNv == manv);
            if (nv == null) return NotFound();

            // Cập nhật thông tin
            nv.TenNv = model.TenNv;
            

            // Nếu có nhập mật khẩu mới thì cập nhật
            if (!string.IsNullOrWhiteSpace(model.MatKhau))
            {
                nv.MatKhau = model.MatKhau;
            }

            db.SaveChanges();
            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("ThongTin");
        }

        [HttpGet]
        public IActionResult ThemNhanVien()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemNhanVien(NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                nhanVien.Role = "staff";
                nhanVien.MatKhau = "888999"; // Bạn có thể mã hoá nếu cần

                db.Add(nhanVien);
                await db.SaveChangesAsync();
                TempData["Message"] = "Đã thêm nhân viên mới thành công.";
                return RedirectToAction(nameof(QuanLyNhanVien));
            }
            return View(nhanVien);
        }
    }


}

