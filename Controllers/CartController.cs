using Microsoft.AspNetCore.Mvc;
using MOBILUX.Data;
using MOBILUX.ViewModels;
using MOBILUX.Helper;
using MOBILUX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MOBILUX.Controllers
{
	public class CartController : Controller
	{
		private readonly MobiluxContext db;

		public CartController(MobiluxContext context)
		{
			db = context;
		}

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
		{
			return View(Cart);
		}
		[Authorize]
		public IActionResult AddToCart(int id, int quantity = 1)
		{
			var gioHang = Cart;
			var item = gioHang.SingleOrDefault(p => p.MaSp == id);
			if (item == null)
			{
				var hangHoa = db.SanPhams.SingleOrDefault(p => p.MaSp == id);
				if (hangHoa == null)
				{
					TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
					return Redirect("/404");
				}
				item = new CartItem
				{
					MaSp = hangHoa.MaSp,
					TenSp = hangHoa.TenSp,
					DonGia = hangHoa.Gia ?? 0,
					Hinh = hangHoa.Hinh ?? string.Empty,
					SoLuong = quantity,
					GiamGia = hangHoa.GiamGia ?? 0
				};
				gioHang.Add(item);
			}
			else
			{
				item.SoLuong += quantity;
			}
			HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
			return RedirectToAction("Index");
			
		}
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaSp == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Tang(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaSp == id);
            if (item != null)
            {
                item.SoLuong += 1;
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Giam(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaSp == id);
            if (item != null)
            {
                item.SoLuong -= 1;
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }








        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Checkout(CheckoutVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var khachHang = new KhachHang();
        //        var staffId = int.Parse(HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_STAFFID).Value);

        //        if (model.giongkhachhang)
        //        {
        //            khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == staffId);
        //        }

        //        var donHang = new DonHang
        //        {
        //            MaKh = staffId,
        //            TenNguoiDat = model.HoTen ?? khachHang.HoTenKh,
        //            DiaChi = model.DiaChi ?? khachHang.DiaChi,
        //            NgayTao = DateTime.Now,
        //            NgayGiao = DateTime.Now.AddDays(3),
        //            CachThanhToan = "Nhận tại quầy",
        //            MaNv = staffId,
        //            GhiChu = model.GhiChu,
        //            TrangThai = "Chờ xử lý",
        //            Gia = Cart.Sum(item => (item.DonGia - item.GiamGia) * item.SoLuong)
        //        };

        //        db.Database.BeginTransaction();
        //        try
        //        {
        //            db.DonHangs.Add(donHang);
        //            await db.SaveChangesAsync();

        //            var chiTietList = new List<ChiTietHd>();
        //            foreach (var item in Cart)
        //            {
        //                var sp = db.SanPhams.SingleOrDefault(x => x.MaSp == item.MaSp);
        //                if (sp == null || sp.SoLuong < item.SoLuong)
        //                {
        //                    throw new Exception($"Sản phẩm không đủ số lượng: {item.TenSp}");
        //                }

        //                sp.SoLuong -= item.SoLuong;

        //                var donGiaSauGiam = item.DonGia - item.GiamGia;
        //                if (donGiaSauGiam < 0)
        //                {
        //                    throw new Exception($"Giảm giá không hợp lệ cho sản phẩm: {item.TenSp}");
        //                }

        //                chiTietList.Add(new ChiTietHd
        //                {
        //                    MaDh = donHang.MaDh,
        //                    MaSp = item.MaSp,
        //                    SoLuong = item.SoLuong,
        //                    Gia = item.DonGia,
        //                    GiamGia = (int?)Math.Round(item.GiamGia) * item.SoLuong,
        //                    ThanhTien = donGiaSauGiam * item.SoLuong
        //                });
        //            }

        //            db.UpdateRange(db.SanPhams); // cập nhật lại số lượng
        //            db.AddRange(chiTietList);
        //            await db.SaveChangesAsync();

        //            db.Database.CommitTransaction();
        //            HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

        //            return View("Success");
        //        }
        //        catch (Exception ex)
        //        {
        //            db.Database.RollbackTransaction();
        //            ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
        //        }
        //    }

        //    return View(Cart);
        //}

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/SanPham");
            }

            return View(Cart);
        }


        [Authorize]
        [HttpGet]  // thêm attribute này nếu chưa có
        public IActionResult GetLoggedInCustomerInfo()
        {
            var staffId = int.Parse(User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_STAFFID)?.Value ?? "0");
            var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == staffId);

            if (khachHang == null)
                return NotFound();

            return Json(new
            {
                hoTenKh = khachHang.HoTenKh,
                diaChi = khachHang.DiaChi,
                email = khachHang.Email,
                sdt = khachHang.Sdt
            });
        }



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                var khachHang = new KhachHang();
                var staffId = int.Parse(HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_STAFFID).Value);

                if (model.giongkhachhang)
                {
                    khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == staffId);
                }

                var donHang = new DonHang
                {
                    MaKh = staffId, // <-- dùng luôn staffId
                    TenNguoiDat = model.HoTen ?? khachHang.HoTenKh,
                    DiaChi = model.DiaChi ?? khachHang.DiaChi,
                    NgayTao = DateTime.Now,
                    NgayGiao = DateTime.Now.AddDays(3),
                    CachThanhToan = "Nhận tại quầy",
                    //MaNv = staffId,
                    GhiChu = model.GhiChu,
                    TrangThai = "Chờ xử lý",
                    Gia = Cart.Sum(item => (item.DonGia - item.GiamGia) * item.SoLuong)
                };

                db.Database.BeginTransaction();
                try
                {
                    db.DonHangs.Add(donHang);
                    await db.SaveChangesAsync();

                    var chiTietList = new List<ChiTietHd>();
                    foreach (var item in Cart)
                    {
                        var sp = db.SanPhams.SingleOrDefault(x => x.MaSp == item.MaSp);
                        if (sp == null || sp.SoLuong < item.SoLuong)
                        {
                            throw new Exception($"Sản phẩm không đủ số lượng: {item.TenSp}");
                        }

                        sp.SoLuong -= item.SoLuong;

                        var donGiaSauGiam = item.DonGia - item.GiamGia;
                        if (donGiaSauGiam < 0)
                        {
                            throw new Exception($"Giảm giá không hợp lệ cho sản phẩm: {item.TenSp}");
                        }

                        chiTietList.Add(new ChiTietHd
                        {
                            MaDh = donHang.MaDh,
                            MaSp = item.MaSp,
                            SoLuong = item.SoLuong,
                            Gia = item.DonGia,
                            GiamGia = (int?)Math.Round(item.GiamGia) * item.SoLuong,
                            ThanhTien = donGiaSauGiam * item.SoLuong
                        });
                    }

                    db.UpdateRange(db.SanPhams);
                    db.AddRange(chiTietList);
                    await db.SaveChangesAsync();

                    db.Database.CommitTransaction();
                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

                    return View("Success");
                }
                catch (Exception ex)
                {
                    db.Database.RollbackTransaction();
                    ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
                }
            }

            return View(Cart);
        }

    }
}
