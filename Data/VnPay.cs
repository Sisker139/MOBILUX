using System;
using System.Collections.Generic;

namespace MOBILUX.Data;

public partial class VnPay
{
    public int MaVnPay { get; set; }

    public int? MaDh { get; set; }

    public string? MaGiaoDich { get; set; }

    public string? PhuongThucThanhToan { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? GhiChu { get; set; }

    public virtual DonHang? MaDhNavigation { get; set; }
}
