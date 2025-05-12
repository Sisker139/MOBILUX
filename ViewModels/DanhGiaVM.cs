using System.ComponentModel.DataAnnotations;

namespace MOBILUX.ViewModels
{
    public class DanhGiaVM
    {
        public int MaSp { get; set; }

        [Required]
        public string HoTen { get; set; }

		[Required(ErrorMessage = "Email không được bỏ trống")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
		public string Email { get; set; }

		[Required]
        public string NoiDung { get; set; }
    }
}
