using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MOBILUX.Data;

public partial class DanhMuc
{

    public int MaDanhMuc { get; set; }

    [Display(Name = "Tên Danh Mục")]

    public string TenDanhMuc { get; set; } = null!;

    [Display(Name = "Trạng Thái")]

    public string? TrangThai { get; set; } = "Hoạt động";

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
