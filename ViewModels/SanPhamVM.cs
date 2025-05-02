namespace MOBILUX.ViewModels
{
    public class SanPhamVM
    {
        public int MaSp {  get; set; }

        public string TenSp { get; set; }

        public string Hinh { get; set; }

        public double DonGia { get; set; }

        public string MotaNgan { get; set; }

        public string TenDanhMuc { get; set; }
    }
	public class PaginatedSanPhamVM
	{
		public IEnumerable<SanPhamVM> SanPhams { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
	}

	public class ChiTietSanPhamVM
	{
		public int MaSp { get; set; }

		public string TenSp { get; set; }

		public string Hinh { get; set; }

		public double DonGia { get; set; }

		public string MotaNgan { get; set; }

		public string TenDanhMuc { get; set; }
		public string Mota { get; set; }
		public string Dungluong { get; set; }
		public string Trongluong { get; set; }
		public string Pin { get; set; }
		public double GiamGia { get; set; }

		public int SoLuong { get; set; }

		public string TrangThai { get; set; }

		public int Spnb { get; set; }
	}
}
