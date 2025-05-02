using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MOBILUX.Data;
using MOBILUX.Helper;
using MOBILUX.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace MOBILUX.Controllers
{
    public class Login : Controller
    {
        private readonly MobiluxContext db;

        public Login(MobiluxContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                // Tìm nhân viên theo MaNv
                var nhanVien = db.NhanViens.SingleOrDefault(nv => nv.MaNv.ToString() == model.UserName);

                if (nhanVien != null)
                {
                    // Nếu tìm thấy nhân viên
                    if (nhanVien.MatKhau != model.Password)
                    {
                        ModelState.AddModelError("loi", "!Sai mật khẩu");
                    }
                    else
                    {
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, nhanVien.TenNv),
                    new Claim(MySetting.CLAIM_ROLE, nhanVien.Role),
                    new Claim(ClaimTypes.Role, nhanVien.Role),
                    new Claim(MySetting.CLAIM_STAFFID, nhanVien.MaNv.ToString())
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(claimsPrincipal);

                        if (Url.IsLocalUrl(ReturnUrl))
                            return Redirect(ReturnUrl);
                        else
                            return Redirect("/");
                    }
                }
                else
                {
                    // Nếu không tìm thấy nhân viên -> tìm khách hàng theo số điện thoại
                    var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.Sdt == model.UserName);

                    if (khachHang == null)
                    {
                        ModelState.AddModelError("loi", "!Sai thông tin đăng nhập");
                    }
                    else
                    {
                        if (khachHang.MatKhau != model.Password)
                        {
                            ModelState.AddModelError("loi", "!Sai mật khẩu");
                        }
                        else
                        {
                            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachHang.HoTenKh),
                        new Claim(MySetting.CLAIM_ROLE, khachHang.Role),
                        new Claim(ClaimTypes.Role, khachHang.Role),
                        new Claim(MySetting.CLAIM_STAFFID, khachHang.MaKh.ToString())
                    };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            if (Url.IsLocalUrl(ReturnUrl))
                                return Redirect(ReturnUrl);
                            else
                                return Redirect("/");
                        }
                    }
                }
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        //private bool NhanVienExists(int id)
        //{
        //    return db.NhanViens.Any(e => e.MaNv == id);
        //}

    }
}
