using System.ComponentModel.DataAnnotations;
namespace MOBILUX.ViewModels
{
    public class RegisterVM
    {
        
       

        [Display(Name ="Tên đăng nhập")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage ="Tối Đa 100 kí tự")]
        public string HoTenKh { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "*")]
        [MaxLength(10, ErrorMessage = "Tối Đa 10 kí tự")]
        [RegularExpression(@"0\d{9}", ErrorMessage = "Nhập Đúng số điện thoại thuộc Việt Nam")]
        public string Sdt { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối Đa 100 kí tự")]
        public string Email { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [MaxLength(50, ErrorMessage = "Tối Đa 50 kí tự")]
        public string MatKhau { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "*")]
        public string DiaChi { get; set; }

        
    }
}
