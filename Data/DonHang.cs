using System;
using System.Collections.Generic;

namespace MOBILUX.Data;

public partial class DonHang
{
    public int MaDh { get; set; }

    public int? MaKh { get; set; }

    public string? TenNguoiDat { get; set; }

    public string? TenSp { get; set; }

    public int? MaSp { get; set; }

    public DateTime NgayTao { get; set; }

    public DateTime NgayGiao { get; set; }

    public double Gia { get; set; }

    public string DiaChi { get; set; } = null!;

    public string? CachThanhToan { get; set; }

    public int? MaNv { get; set; }

    public string? GhiChu { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<ChiTietHd> ChiTietHds { get; set; } = new List<ChiTietHd>();

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; }

    public virtual SanPham? MaSpNavigation { get; set; }

    public virtual ICollection<VnPay> VnPays { get; set; } = new List<VnPay>();
}
