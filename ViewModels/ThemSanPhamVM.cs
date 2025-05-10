using MOBILUX.Data;
using System.ComponentModel.DataAnnotations;

namespace MOBILUX.ViewModels
{
    public class ThemSanPhamVM
    {

       

        [Display(Name = "Tên Sản Phẩm")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 kí tự")]
        public string TenSP { get; set; } = null!;

        [Display(Name = "Mã loại sản phẩm")]
        [Required(ErrorMessage = "*")]
        public int MaDanhMuc { get; set; }

        [Display(Name = "Mô tả")]
        [MaxLength(5000, ErrorMessage = "Tối đa 5000 kí tự")] 
        public string? MoTa { get; set; }

        [Display(Name = "Mô tả ngắn ")]
        [MaxLength(5000, ErrorMessage = "Tối đa 5000 kí tự")] 
        public string? MoTaNgan { get; set; }

        [Display(Name = " Giá")]
        [Required(ErrorMessage = "*")]
        [Range(0, double.MaxValue, ErrorMessage = " giá phải lớn hơn hoặc bằng 0")]
        public double? Gia { get; set; }

        [Display(Name = "Giảm Giá")]
        [Required(ErrorMessage = "*")]
        [Range(0,  9999999, ErrorMessage = "Giảm giá phải trong khoảng từ 0 đến 9999999")]
        public double GiamGia { get; set; } 

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "*")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int SoLuong { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? Hinh { get; set; }

        [Display(Name = "Dung Lượng")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string? DungLuong { get; set; }


        [Display(Name = "Trọng Lượng")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string? TrongLuong { get; set; }


        [Display(Name = "Pin")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string? Pin { get; set; }


        [Display(Name = "Trạng Thái")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 kí tự")]
        public string? TrangThai { get; set; }

        // public virtual LoaiSp MaLoaiSpNavigation { get; set; } = null!;
        public List<DanhMuc> ListDM { get; set; } = new List<DanhMuc>();
    }
}
