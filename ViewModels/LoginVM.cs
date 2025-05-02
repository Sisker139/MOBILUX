using System.ComponentModel.DataAnnotations;

namespace MOBILUX.ViewModels
{
	public class LoginVM
	{

		[Display(Name = "Mã đăng nhập hoặc số điện thoại")]
		[Required(ErrorMessage = "Chưa nhập tên đăng nhập")]
		[MaxLength(10, ErrorMessage = "Tối đa 10 kí tự")]
		public string UserName { get; set; }


		[Display(Name = "Mật khẩu")]
		[Required(ErrorMessage = "Chưa nhập mật khẩu")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

	}
}
