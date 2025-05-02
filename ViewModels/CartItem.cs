namespace MOBILUX.ViewModels
{
	public class CartItem
	{
		public int MaSp { get; set; }


		
		public string Hinh { get; set; }
		public string TenSp { get; set; }

		public double DonGia { get; set; }

		public double GiamGia { get; set; }
		public int SoLuong { get; set; }

		public double ThanhTien => SoLuong * (DonGia - GiamGia);

	}
}
