
GO
ALTER PROCEDURE dbo.sp_HO_SO_CapNhatHoSo
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
		Ngay_Nhan_Don=@NgayNhanDon, Ma_Nguoi_Cap_Nhat=@MaNguoiCapNhat,AssigneeId=@HoSoCuaAi,Ngay_Cap_Nhat=@NgayCapNhat,
		Ma_Trang_Thai=@MaTrangThai,San_Pham_Vay=@SanPhamVay,Co_Bao_Hiem=@CoBaoHiem,
		So_Tien_Vay=@SoTienVay,Han_Vay=@HanVay,Ten_Cua_Hang=@TenCuaHang,
		Ma_Ket_Qua=@KetQuaHS,
		BirthDay = @birthDay,
    CMNDDay= @cmndDay,
	DisbursementDate =@DisbursementDate
	WHERE ID=@ID
END













GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[sp_HO_SO_LayChiTiet]'
GO
ALTER PROCEDURE dbo.sp_HO_SO_LayChiTiet
	-- Add the parameters for the stored procedure here
	@ID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select hs.ID
	,Ma_Ho_So as MaHoSo
	,Ten_Khach_Hang as TenKhachHang
	,CMND,Dia_Chi as DiaChi
	,Courier_Code as CourierCode,
	SDT,SDT2
	,Gioi_Tinh as GioiTinh
	,hs.CreatedTime as NgayTao
	,hs.CreatedBy as MaNguoiTao,
	hs.DisbursementDate,
	Ma_Khu_Vuc as MaKhuVuc
	, tt.Ten as TenTrangThai
	, kq.Ten as KetQuaText,
	AssigneeId as HoSoCuaAi
	,Ngay_Cap_Nhat as NgayCapNhat
	,Ma_Nguoi_Cap_Nhat as MaNguoiCapNhat,
	Ngay_Nhan_Don as NgayNhanDon
	,Ma_Trang_Thai as MaTrangThai
	,Ma_Ket_Qua as MaKetQua,San_Pham_Vay as SanPhamVay
	,sv.Ten as ProductName
	,Ten_Cua_Hang as TenCuaHang
	,Co_Bao_Hiem as CoBaoHiem,So_Tien_Vay as SoTienVay,Han_Vay as HanVay,Ghi_Chu as GhiChu
	,BirthDay,CmndDay,
	dbo.getDoitacBySanphamId(sv.ID,0) as Doitac,
	dbo.getDoitacBySanphamId(sv.ID,1) as PartnerId,
	sv.Ten as Sanpham,
	sv.Ma as ProductCode
	,dbo.getProvinceNameFromDistrictId(hs.Ma_Khu_Vuc) as ProvinceName
	,dbo.getProvinceIdFromDistrictId(hs.Ma_Khu_Vuc) as ProvinceId
	,kv.Ten as DistrictName,
	nv.Ten_Dang_Nhap as SaleCode,
	nv.Ho_Ten as EmployeeName,
	nv.Dien_Thoai as EmployeePhone,
	 fintechcom_vn_PortalNew.fn_getGhichuByHosoId(hs.ID,1) as GhiChu
	from HO_SO hs
	left join TRANG_THAI_HS tt on tt.ID=hs.Ma_Trang_Thai
	left join KET_QUA_HS kq on kq.ID=hs.Ma_Ket_Qua
	left join SAN_PHAM_VAY sv on hs.San_Pham_Vay = sv.ID
	left join KHU_VUC kv on hs.Ma_Khu_Vuc = kv.ID
	left join NHAN_VIEN nv on hs.AssigneeId = nv.ID
	 where hs.ID=@ID
END



GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[sp_HO_SO_Them]'
GO
ALTER PROCEDURE dbo.sp_HO_SO_Them
@ID int out,
@MaHoSo nvarchar(50),
@TenKhachHang nvarchar(50),
@CMND nvarchar(50),
@DiaChi ntext,
@CourierCode int,
@MaKhuVuc int,
@SDT nvarchar(50),
@SDT2 nvarchar(50),
@GioiTinh int,
@NgayTao datetime,
@MaNguoiTao int,
@HoSoCuaAi int,
@NgayNhanDon datetime,
@MaTrangThai int,
@KetQuaHS int,
@SanPhamVay int,
@CoBaoHiem int,
@SoTienVay decimal,
@HanVay int,
@TenCuaHang nvarchar(200),
@birthDay datetime,
@cmndDay datetime,
@DisbursementDate datetime = null
AS
BEGIN
 INSERT HO_SO
 (Ma_Ho_So, Ten_Khach_Hang,CMND,Dia_Chi, Courier_Code,Ma_Khu_Vuc,SDT,SDT2,Gioi_Tinh,CreatedTime,CreatedBy,AssigneeId
 ,Ngay_Nhan_Don, Ma_Trang_Thai, Ma_Ket_Qua,San_Pham_Vay,Ten_Cua_Hang,Co_Bao_Hiem,So_Tien_Vay,Han_Vay,Da_Xoa, Ngay_Cap_Nhat,
 BirthDay,CMNDDay,DisbursementDate)
 VALUES(@MaHoSo,Upper(@TenKhachHang),@CMND,@DiaChi,@CourierCode,@MaKhuVuc,@SDT,@SDT2,@GioiTinh,@NgayTao,@MaNguoiTao,
 @HoSoCuaAi,@NgayNhanDon,@MaTrangThai,@KetQuaHS,@SanPhamVay,@TenCuaHang,@CoBaoHiem,@SoTienVay,@HanVay,0, @NgayTao,
 @birthDay,@cmndDay,@DisbursementDate) 
	SET @ID=@@IDENTITY
END

