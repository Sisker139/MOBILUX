using System;
using System.Collections.Generic;

namespace MOBILUX.Data;

public partial class ChiTietHd
{
    public int MaCtdh { get; set; }

    public int? MaDh { get; set; }

    public int MaSp { get; set; }

    public double? Gia { get; set; }

    public int? SoLuong { get; set; }

    public double? GiamGia { get; set; }

    public double? ThanhTien { get; set; }

    public virtual DonHang? MaDhNavigation { get; set; }

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
