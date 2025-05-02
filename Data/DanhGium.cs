using System;
using System.Collections.Generic;

namespace MOBILUX.Data;

public partial class DanhGium
{
    public int MaDg { get; set; }

    public int? MaKh { get; set; }

    public int MaSp { get; set; }

    public string Hoten { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime NgayTao { get; set; }

    public double? Sao { get; set; }

    public string? NoiDung { get; set; }

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
