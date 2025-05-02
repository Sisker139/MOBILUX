using System;
using System.Collections.Generic;

namespace MOBILUX.Data;

public partial class KhachHang
{
    public int MaKh { get; set; }

    public string HoTenKh { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public string? Email { get; set; }

    public string MatKhau { get; set; } = null!;

    public string? DiaChi { get; set; }

	public string Role { get; set; } = "customer";

	public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<LienHe> LienHes { get; set; } = new List<LienHe>();
}
