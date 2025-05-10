using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MOBILUX.ViewModels
{
	public class CheckoutVM
	{
		public bool giongkhachhang { get; set; } = true;
		public string? HoTen {  get; set; }


        [BindNever]
        public int? Makh { get; set; }
		public string? DiaChi { get; set; }

		public int? MaNv { get; set; }

		public string?  GhiChu { get; set; }

        public string? Email { get; set; }
        public string? Sdt { get; set; }
    }
}
