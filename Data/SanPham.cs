using System;
using System.Collections.Generic;

namespace MOBILUX.Data;

public partial class SanPham
{
    public int MaSp { get; set; }

    public string TenSp { get; set; } = null!;

    public int MaDanhMuc { get; set; }

    public string? MoTa { get; set; }

    public string? MoTaNgan { get; set; }

    public double? Gia { get; set; }

    public string? Hinh { get; set; }

    public string? DungLuong { get; set; }

    public string? TrongLuong { get; set; }

    public string? Pin { get; set; }

    public double? GiamGia { get; set; }

    public int? SoLuong { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<ChiTietHd> ChiTietHds { get; set; } = new List<ChiTietHd>();

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual DanhMuc MaDanhMucNavigation { get; set; } = null!;
}
