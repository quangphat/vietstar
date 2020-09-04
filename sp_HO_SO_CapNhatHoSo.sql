USE [PortalPro]
GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_CapNhatHoSo]    Script Date: 05/09/2020 12:05:10 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_HO_SO_CapNhatHoSo]
@ID int,
@TenKhachHang nvarchar(50),
@CMND nvarchar(50),
@DiaChi ntext,
@CourierCode int,
@MaKhuVuc int,
@SDT nvarchar(50),
@SDT2 nvarchar(50),
@GioiTinh int,
@MaNguoiCapNhat int,
@HoSoCuaAi int,
@NgayNhanDon datetime,
@NgayCapNhat datetime,
@MaTrangThai int,
@SanPhamVay int,
@CoBaoHiem int,
@SoTienVay decimal,
@HanVay int,
@KetQuaHS int,
@TenCuaHang nvarchar(200) = '',
@birthDay datetime,
@cmndDay datetime,
@DisbursementDate datetime = null
AS
BEGIN
	UPDATE HO_SO SET Ten_Khach_Hang=Upper(@TenKhachHang),
		CMND=@CMND,Dia_Chi=@DiaChi, Courier_Code=@CourierCode,Ma_Khu_Vuc=@MaKhuVuc,SDT=@SDT,SDT2=@SDT2,Gioi_Tinh=@GioiTinh,
		Ngay_Nhan_Don=@NgayNhanDon, UpdatedBy=@MaNguoiCapNhat,AssigneeId=@HoSoCuaAi,UpdatedTime=@NgayCapNhat,
		Ma_Trang_Thai=@MaTrangThai,San_Pham_Vay=@SanPhamVay,Co_Bao_Hiem=@CoBaoHiem,
		So_Tien_Vay=@SoTienVay,Han_Vay=@HanVay,Ten_Cua_Hang=@TenCuaHang,
		Ma_Ket_Qua=@KetQuaHS,
		BirthDay = @birthDay,
    CMNDDay= @cmndDay,
	DisbursementDate =@DisbursementDate
	WHERE ID=@ID
END













