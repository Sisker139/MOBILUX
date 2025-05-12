using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MOBILUX.Data;

public partial class MobiluxContext : DbContext
{
    public MobiluxContext()
    {
    }

    public MobiluxContext(DbContextOptions<MobiluxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TinNhanLienHe> TinNhanLienHes { get; set; }

	public virtual DbSet<ChiTietHd> ChiTietHds { get; set; }
	public virtual DbSet<DanhGium> DanhGia { get; set; }
	public virtual DbSet<DanhMuc> DanhMucs { get; set; }
	public virtual DbSet<DonHang> DonHangs { get; set; }
	public virtual DbSet<KhachHang> KhachHangs { get; set; }
	public virtual DbSet<LienHe> LienHes { get; set; }
	public virtual DbSet<NhanVien> NhanViens { get; set; }
	public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<VnPay> VnPays { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
		modelBuilder.Entity<ChiTietHd>(entity =>
		{
			entity.HasKey(e => e.MaCtdh).HasName("PK__ChiTietH__1E4E40F07B081B04");

			entity.ToTable("ChiTietHD");

			entity.Property(e => e.MaCtdh).HasColumnName("MaCTDH");
			entity.Property(e => e.MaDh).HasColumnName("MaDH");
			entity.Property(e => e.MaSp).HasColumnName("MaSP");

			entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ChiTietHds)
				.HasForeignKey(d => d.MaDh)
				.HasConstraintName("FK__ChiTietHD__MaDH__00200768");

			entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietHds)
				.HasForeignKey(d => d.MaSp)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__ChiTietHD__MaSP__01142BA1");
		});

		modelBuilder.Entity<DanhGium>(entity =>
		{
			entity.HasKey(e => e.MaDg).HasName("PK__DanhGia__272586609109ED97");

			entity.Property(e => e.MaDg).HasColumnName("MaDG");
			entity.Property(e => e.Email).HasMaxLength(100);
			entity.Property(e => e.Hoten).HasMaxLength(50);
			entity.Property(e => e.MaKh).HasColumnName("MaKH");
			entity.Property(e => e.MaSp).HasColumnName("MaSP");
			entity.Property(e => e.NgayTao).HasColumnType("datetime");
			entity.Property(e => e.Sao).HasColumnName("sao");

			entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DanhGia)
				.HasForeignKey(d => d.MaKh)
				.HasConstraintName("FK__DanhGia__MaKH__03F0984C");

			entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.DanhGia)
				.HasForeignKey(d => d.MaSp)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__DanhGia__MaSP__04E4BC85");
		});

		modelBuilder.Entity<DanhMuc>(entity =>
		{
			entity.HasKey(e => e.MaDanhMuc).HasName("PK__DanhMuc__B3750887D1AC0B6D");

			entity.ToTable("DanhMuc");

			entity.Property(e => e.TenDanhMuc).HasMaxLength(50);
			entity.Property(e => e.TrangThai).HasMaxLength(50);
		});

		modelBuilder.Entity<DonHang>(entity =>
		{
			entity.HasKey(e => e.MaDh).HasName("PK__DonHang__27258661885AF5B5");

			entity.ToTable("DonHang");

			entity.Property(e => e.MaDh).HasColumnName("MaDH");
			entity.Property(e => e.CachThanhToan).HasMaxLength(50);
			entity.Property(e => e.DiaChi).HasMaxLength(100);
			entity.Property(e => e.MaKh).HasColumnName("MaKH");
			entity.Property(e => e.MaNv).HasColumnName("MaNV");
			entity.Property(e => e.MaSp).HasColumnName("MaSP");
			entity.Property(e => e.NgayGiao).HasColumnType("datetime");
			entity.Property(e => e.NgayTao).HasColumnType("datetime");
			entity.Property(e => e.TenNguoiDat).HasMaxLength(100);
			entity.Property(e => e.TenSp)
				.HasMaxLength(100)
				.HasColumnName("TenSP");
			entity.Property(e => e.TrangThai).HasMaxLength(50);

			entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DonHangs)
				.HasForeignKey(d => d.MaKh)
				.HasConstraintName("FK__DonHang__MaKH__7C4F7684");

			entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.DonHangs)
				.HasForeignKey(d => d.MaNv)
				.HasConstraintName("FK__DonHang__MaNV__7D439ABD");

			entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.DonHangs)
				.HasForeignKey(d => d.MaSp)
				.HasConstraintName("FK_DonHang_SanPham");
		});

		modelBuilder.Entity<KhachHang>(entity =>
		{
			entity.HasKey(e => e.MaKh).HasName("PK__khachHan__2725CF1EBBBDBDFE");

			entity.ToTable("khachHang");

			entity.Property(e => e.MaKh).HasColumnName("MaKH");
			entity.Property(e => e.Email)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.HoTenKh)
				.HasMaxLength(100)
				.HasColumnName("HoTenKH");
			entity.Property(e => e.MatKhau).HasMaxLength(50);
			entity.Property(e => e.Role)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.Sdt)
				.HasMaxLength(10)
				.IsUnicode(false);
		});

		modelBuilder.Entity<LienHe>(entity =>
		{
			entity.HasKey(e => e.MaLh).HasName("PK__LienHe__2725C77FCB101B7A");

			entity.ToTable("LienHe");

			entity.Property(e => e.MaLh).HasColumnName("MaLH");
			entity.Property(e => e.Hoten).HasMaxLength(50);
			entity.Property(e => e.MaKh).HasColumnName("MaKH");
			entity.Property(e => e.NgayTao).HasColumnType("datetime");
			entity.Property(e => e.TrangThai).HasMaxLength(50);

			entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.LienHes)
				.HasForeignKey(d => d.MaKh)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__LienHe__MaKH__07C12930");
		});

		modelBuilder.Entity<NhanVien>(entity =>
		{
			entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70A5FC6A1B6");

			entity.ToTable("NhanVien");

			entity.Property(e => e.MaNv).HasColumnName("MaNV");
			entity.Property(e => e.MatKhau).HasMaxLength(50);
			entity.Property(e => e.Role)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.TenNv)
				.HasMaxLength(100)
				.HasColumnName("TenNV");
		});

		modelBuilder.Entity<SanPham>(entity =>
		{
			entity.HasKey(e => e.MaSp).HasName("PK__SanPham__2725081C1FF115BE");

			entity.ToTable("SanPham");

			entity.Property(e => e.MaSp).HasColumnName("MaSP");
			entity.Property(e => e.DungLuong)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.Pin)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.TenSp)
				.HasMaxLength(100)
				.HasColumnName("TenSP");
			entity.Property(e => e.TrangThai).HasMaxLength(50);
			entity.Property(e => e.TrongLuong)
				.HasMaxLength(50)
				.IsUnicode(false);

			entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.SanPhams)
				.HasForeignKey(d => d.MaDanhMuc)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__SanPham__MaDanhM__797309D9");
		});

		modelBuilder.Entity<TinNhanLienHe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TinNhanL__3214EC07B1DC830C");

            entity.ToTable("TinNhanLienHe");

            entity.Property(e => e.NguoiGui).HasMaxLength(50);
            entity.Property(e => e.ThoiGianGui)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

			entity.HasOne(e => e.LienHe)
				.WithMany(lh => lh.TinNhans)
				.HasForeignKey(e => e.MaLh)
				.HasConstraintName("FK_TinNhanLienHe_LienHe");

		});

        modelBuilder.Entity<VnPay>(entity =>
        {
            entity.HasKey(e => e.MaVnPay).HasName("PK__VnPay__B80122AF4855DF19");

            entity.ToTable("VnPay");

            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaGiaoDich).HasMaxLength(100);
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.PhuongThucThanhToan).HasMaxLength(100);

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.VnPays)
                .HasForeignKey(d => d.MaDh)
                .HasConstraintName("FK__VnPay__MaDH__619B8048");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
