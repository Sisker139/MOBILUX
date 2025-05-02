using System;
using System.Collections.Generic;

namespace MOBILUX.Data;

public partial class LienHe
{
    public int MaLh { get; set; }

    public int MaKh { get; set; }

    public string Hoten { get; set; } = null!;

    public string? NoiDung { get; set; }

    public DateTime NgayTao { get; set; }

    public string? TrangThai { get; set; }

    public virtual KhachHang MaKhNavigation { get; set; } = null!;
}
