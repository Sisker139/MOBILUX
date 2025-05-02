using System;
using System.Collections.Generic;

namespace MOBILUX.Data;

public partial class NhanVien
{
    public int MaNv { get; set; }

    public string TenNv { get; set; } = null!;

    public string? Role { get; set; }

    public string MatKhau { get; set; } = null!;

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
