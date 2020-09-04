
GO
ALTER PROCEDURE dbo.sp_HO_SO_Count_TimHoSoDuyet 
	-- Add the parameters for the stored procedure here
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@LoaiNgay int,
	@freeText as nvarchar(50) = '',
	@TrangThai nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if @freeText = '' begin set @freeText = null end;
	set @TrangThai+= '0,19'
	Declare @DSNhomQL TABLE(ID int)
	if(@MaNhom = 0)
	Begin
	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where (Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.')
	 Where CONVERT(int, value) in
	(
		Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap
	)) > 0
	or NHOM.ID in (Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap)
	End
	Select count(*) as TotalRecord
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and ((@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
		Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM
		 Where NHAN_VIEN_NHOM.Ma_Nhom 
		 in (Select NHOM.ID From NHOM
		  Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID))
		  like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
		  or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) 
		  like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)))
		or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao 
		in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)))
		)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@CMND+'%'
	and ((HO_SO.CreatedTime between CONVERT(date, @TuNgay) 
	and CONVERT(date, @DenNgay) 
	and @LoaiNgay = 1) or (HO_SO.Ngay_Cap_Nhat between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ',')) 
	and (@freeText is null or HO_SO.Ma_Ho_So like N'%' + @freeText + '%'
		or DOI_TAC.Ten like N'%' + @freeText + '%'
		or HO_SO.CMND like N'%' + @freeText + '%'
		or HO_SO.Ten_Khach_Hang like N'%' + @freeText + '%'
		or TRANG_THAI_HS.Ten like N'%' + @freeText + '%'
		or KET_QUA_HS.Ten like N'%' + @freeText + '%'
		or NV1.Ma like N'%' + @freeText + '%'
		or NV1.Ho_Ten like N'%' + @freeText + '%'
		or NV2.Ma like N'%' + @freeText + '%'
		or NV3.Ma like N'%' + @freeText + '%'
		or kvt.Ten like N'%' + @freeText + '%'
		or SAN_PHAM_VAY.Ten like N'%' + @freeText + '%'
	)
END





GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[sp_HO_SO_Count_TimHoSoQuanLy]'
GO
ALTER PROCEDURE dbo.sp_HO_SO_Count_TimHoSoQuanLy 
	-- Add the parameters for the stored procedure here
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@TrangThai nvarchar(50),
	@freeText as nvarchar(50) = '',
	@LoaiNgay int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if @freeText = '' begin set @freeText = null end;
	if @MaHS is null begin set @MaHS ='' end;
	if @CMND is null begin set @CMND ='' end;
	set @TrangThai += ',0,19'
	Declare @DSNhomQL TABLE(ID int)
	if(@MaNhom = 0)
	Begin
	-- Lay nhom la thanh vien
	Insert Into @DSNhomQL Select NHAN_VIEN_NHOM.Ma_Nhom From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = @MaNVDangNhap
	-- Lay nhom con truc tiep (nv la nguoi quan ly)
	Insert Into @DSNhomQL Select NHOM.ID From NHOM Where NHOM.Ma_Nhom_Cha in (select * from @DSNhomQL)
	-- Lay ds nhom tu nhom quan ly truc tiep tro xuong
	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where (Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.') Where CONVERT(int, value) in
	(
		Select * From @DSNhomQL
	)) > 0
	End
	Select count(*) as TotalRecord
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and (
			(@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) 
			or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
			Select NHAN_VIEN_NHOM.Ma_Nhan_Vien 
			From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in 
			(Select NHOM.ID From NHOM Where 
			((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
			or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)
			))
			or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 
			Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)) and @MaThanhVien = 0)
	)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and (HO_SO.Ma_Ho_So like '%'+@MaHS+'%')
	and (HO_SO.CMND like '%'+@CMND+'%')
	and ((convert(varchar(10), HO_SO.CreatedTime,121) between CONVERT(varchar(10), @TuNgay,121) and CONVERT(varchar(10), @DenNgay,121) and @LoaiNgay = 1) 
	or ( convert(varchar(10),HO_SO.Ngay_Cap_Nhat,121) between CONVERT(varchar(10), @TuNgay,121) and CONVERT(varchar(10), @DenNgay,121) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	and (@freeText is null or HO_SO.Ma_Ho_So like N'%' + @freeText + '%'
		or DOI_TAC.Ten like N'%' + @freeText + '%'
		or HO_SO.CMND like N'%' + @freeText + '%'
		or HO_SO.Ten_Khach_Hang like N'%' + @freeText + '%'
		or TRANG_THAI_HS.Ten like N'%' + @freeText + '%'
		or KET_QUA_HS.Ten like N'%' + @freeText + '%'
		or NV1.Ma like N'%' + @freeText + '%'
		or NV1.Ho_Ten like N'%' + @freeText + '%'
		or NV2.Ma like N'%' + @freeText + '%'
		or NV3.Ma like N'%' + @freeText + '%'
		or kvt.Ten like N'%' + @freeText + '%'
		or SAN_PHAM_VAY.Ten like N'%' + @freeText + '%'
	)
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
	,hs.Ma_Nguoi_Tao as MaNguoiTao,
	hs.DisbursementDate,
	Ma_Khu_Vuc as MaKhuVuc
	, tt.Ten as TenTrangThai
	, kq.Ten as KetQuaText,
	Ho_So_Cua_Ai as HoSoCuaAi
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
	left join NHAN_VIEN nv on hs.Ho_So_Cua_Ai = nv.ID
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
 (Ma_Ho_So, Ten_Khach_Hang,CMND,Dia_Chi, Courier_Code,Ma_Khu_Vuc,SDT,SDT2,Gioi_Tinh,CreatedTime,Ma_Nguoi_Tao,Ho_So_Cua_Ai
 ,Ngay_Nhan_Don, Ma_Trang_Thai, Ma_Ket_Qua,San_Pham_Vay,Ten_Cua_Hang,Co_Bao_Hiem,So_Tien_Vay,Han_Vay,Da_Xoa, Ngay_Cap_Nhat,
 BirthDay,CMNDDay,DisbursementDate)
 VALUES(@MaHoSo,Upper(@TenKhachHang),@CMND,@DiaChi,@CourierCode,@MaKhuVuc,@SDT,@SDT2,@GioiTinh,@NgayTao,@MaNguoiTao,
 @HoSoCuaAi,@NgayNhanDon,@MaTrangThai,@KetQuaHS,@SanPhamVay,@TenCuaHang,@CoBaoHiem,@SoTienVay,@HanVay,0, @NgayTao,
 @birthDay,@cmndDay,@DisbursementDate) 
	SET @ID=@@IDENTITY
END

























GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[sp_HO_SO_TimHoSoCuaToi]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE dbo.sp_HO_SO_TimHoSoCuaToi 
	-- Add the parameters for the stored procedure here
	@MaNhanVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@SDT nvarchar(50),
	@TrangThai nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.CreatedTime as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, HO_SO.Dia_Chi as DiaChiKH,kvt.Ten as KhuVucText, HO_SO.Ghi_Chu as GhiChu 
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left  join SAN_PHAM_VAY on HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
		left join DOI_TAC on SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
		left join TRANG_THAI_HS on  HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	    left join KET_QUA_HS on HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	Where 
	 HO_SO.Ma_Nguoi_Tao = @MaNhanVien
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@SDT+'%'
	and (HO_SO.CreatedTime between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay))
	and HO_SO.Da_Xoa = 0 
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	order by HO_SO.CreatedTime desc
END







GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[sp_HO_SO_TimHoSoCuaToiChuaXem]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE dbo.sp_HO_SO_TimHoSoCuaToiChuaXem 
	-- Add the parameters for the stored procedure here
	@MaNhanVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@SDT nvarchar(50),
	@TrangThai nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.CreatedTime as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, HO_SO.Dia_Chi as DiaChiKH,kvt.Ten as KhuVucText, HO_SO.Ghi_Chu as GhiChu 
	From HO_SO_XEM,HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left  join SAN_PHAM_VAY on HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
		left join DOI_TAC on SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
		left join TRANG_THAI_HS on  HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	    left join KET_QUA_HS on HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	Where 
	HO_SO_XEM.Xem=0
	and HO_SO_XEM.Ma_Ho_So=HO_SO.ID
	and HO_SO.Ma_Nguoi_Tao = @MaNhanVien
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@SDT+'%'
	and (HO_SO.CreatedTime between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay))
	and HO_SO.Da_Xoa = 0 
	--and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	order by HO_SO.CreatedTime desc
END







GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[sp_HO_SO_TimHoSoDuyet]'
GO

  ALTER PROCEDURE dbo.sp_HO_SO_TimHoSoDuyet 
	-- Add the parameters for the stored procedure here
	@offset int,
	@limit int,
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@LoaiNgay int,
	@freeText as nvarchar(50)='',
	@TrangThai nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if @freeText = '' begin set @freeText = null end;
	if CHARINDEX(',6',@TrangThai) >0
	begin
	 set @TrangThai+=',19'
	end
	Declare @DSNhomQL TABLE(ID int)
	if(@MaNhom = 0)
	Begin
	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where (Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.')
	 Where CONVERT(int, value) in
	(
		Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap
	)) > 0
	or NHOM.ID in (Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap)
	End
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo,HO_SO.DisbursementDate, HO_SO.CreatedTime as NgayTao,isnull(HO_SO.F88Result,0) as F88Result,
	 DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,
	 HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS,
	  KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, 
	  NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua,
	   HO_SO.Co_Bao_Hiem as CoBaoHiem, kvt.Ten as DiaChiKH, fintechcom_vn_PortalNew.fn_getGhichuByHosoId(HO_SO.ID,1) as GhiChu, 
	   NV3.Ma as MaNVLayHS,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM
	 Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) = 0)
	  THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM 
	  Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao)
	   END 
	   as DoiNguBanHang, SAN_PHAM_VAY.Ten as TenSanPham
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and ((@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
		Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM
		 Where NHAN_VIEN_NHOM.Ma_Nhom 
		 in (Select NHOM.ID From NHOM
		  Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID))
		  like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
		  or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) 
		  like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)))
		or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao 
		in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)))
		)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@CMND+'%'
	and ((HO_SO.CreatedTime between CONVERT(date, @TuNgay) 
	and CONVERT(date, @DenNgay) 
	and @LoaiNgay = 1) or (HO_SO.Ngay_Cap_Nhat between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	and (@freeText is null or HO_SO.Ma_Ho_So like N'%' + @freeText + '%'
		or DOI_TAC.Ten like N'%' + @freeText + '%'
		or HO_SO.CMND like N'%' + @freeText + '%'
		or HO_SO.Ten_Khach_Hang like N'%' + @freeText + '%'
		or TRANG_THAI_HS.Ten like N'%' + @freeText + '%'
		or KET_QUA_HS.Ten like N'%' + @freeText + '%'
		or NV1.Ma like N'%' + @freeText + '%'
		or NV1.Ho_Ten like N'%' + @freeText + '%'
		or NV2.Ma like N'%' + @freeText + '%'
		or NV3.Ma like N'%' + @freeText + '%'
		or kvt.Ten like N'%' + @freeText + '%'
		or SAN_PHAM_VAY.Ten like N'%' + @freeText + '%'
	)
	order by HO_SO.Ngay_Cap_Nhat desc 
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
END











GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[sp_HO_SO_TimHoSoDuyetChuaXem]'
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE dbo.sp_HO_SO_TimHoSoDuyetChuaXem 
	-- Add the parameters for the stored procedure here
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@LoaiNgay int,
	@TrangThai nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Declare @DSNhomQL TABLE(ID int)
	if(@MaNhom = 0)
	Begin
	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where (Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.') Where CONVERT(int, value) in
	(
		Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap
	)) > 0
	or NHOM.ID in (Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap)
	End
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.CreatedTime as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, kvt.Ten as DiaChiKH, HO_SO.Ghi_Chu as GhiChu, NV3.Ma as MaNVLayHS,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) = 0) THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) END as DoiNguBanHang
	From HO_SO_DUYET_XEM,HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where
	HO_SO_DUYET_XEM.Xem=0
	and HO_SO_DUYET_XEM.Ma_Ho_So=HO_SO.ID
	and HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and ((@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
		Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in (Select NHOM.ID From NHOM Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)))
		or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)))
		)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@CMND+'%'
	and ((HO_SO.CreatedTime between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 1) or (HO_SO.Ngay_Cap_Nhat between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
END




GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[sp_HO_SO_TimHoSoQuanLy]'
GO

  ALTER PROCEDURE dbo.sp_HO_SO_TimHoSoQuanLy 
	-- Add the parameters for the stored procedure here
	@offset int,
	@limit int,
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@TrangThai nvarchar(50),
	@freeText as nvarchar(50) = '',
	@LoaiNgay int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if @freeText = '' begin set @freeText = null end;
	if @MaHS is null begin set @MaHS ='' end;
	if @CMND is null begin set @CMND ='' end;
	if CHARINDEX(',6',@TrangThai) >0
	begin
	 set @TrangThai+=',19'
	end
	Declare @DSNhomQL TABLE(ID int)
	if(@MaNhom = 0)
	Begin
	-- Lay nhom la thanh vien
	Insert Into @DSNhomQL Select NHAN_VIEN_NHOM.Ma_Nhom From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = @MaNVDangNhap
	-- Lay nhom con truc tiep (nv la nguoi quan ly)
	Insert Into @DSNhomQL Select NHOM.ID From NHOM Where NHOM.Ma_Nhom_Cha in (select * from @DSNhomQL)
	-- Lay ds nhom tu nhom quan ly truc tiep tro xuong
	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where 
	(Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.') 
		Where CONVERT(int, value) in (Select * From @DSNhomQL)) > 0
	End
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo,HO_SO.DisbursementDate, HO_SO.CreatedTime as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND,
	 HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai ,TRANG_THAI_HS.Ten as TrangThaiHS,
	 KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang,
	  NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, HO_SO.Dia_Chi as DiaChiKH,
	  isnull(HO_SO.SDT2,'') as Phone,
	  kvt.Ten as KhuVucText, fintechcom_vn_PortalNew.fn_getGhichuByHosoId(HO_SO.ID,1) as GhiChu, NV3.Ma as MaNVLayHS,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM 
	Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) = 0) 
	THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM 
	Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao)
	 END as DoiNguBanHang, 
	 SAN_PHAM_VAY.Ten as TenSanPham
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY,TRANG_THAI_HS, KET_QUA_HS
	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and (
			(@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) 
			or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
			Select NHAN_VIEN_NHOM.Ma_Nhan_Vien 
			From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in 
			(Select NHOM.ID From NHOM Where 
			((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
			or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)
			))
			or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 
			Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)) and @MaThanhVien = 0)
	)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and (HO_SO.Ma_Ho_So like '%'+@MaHS+'%')
	and (HO_SO.CMND like '%'+@CMND+'%')
	and ((convert(varchar(10), HO_SO.CreatedTime,121) between CONVERT(varchar(10), @TuNgay,121) and CONVERT(varchar(10), @DenNgay,121) and @LoaiNgay = 1) 
	or ( convert(varchar(10),HO_SO.Ngay_Cap_Nhat,121) between CONVERT(varchar(10), @TuNgay,121) and CONVERT(varchar(10), @DenNgay,121) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	and (@freeText is null or HO_SO.Ma_Ho_So like N'%' + @freeText + '%'
		or DOI_TAC.Ten like N'%' + @freeText + '%'
		or HO_SO.CMND like N'%' + @freeText + '%'
		or HO_SO.SDT like N'%' + @freeText + '%'
		or HO_SO.Ten_Khach_Hang like N'%' + @freeText + '%'
		or KET_QUA_HS.Ten like N'%' + @freeText + '%'
		or NV1.Ma like N'%' + @freeText + '%'
		or NV1.Ho_Ten like N'%' + @freeText + '%'
		or NV2.Ma like N'%' + @freeText + '%'
		or NV3.Ma like N'%' + @freeText + '%'
		or kvt.Ten like N'%' + @freeText + '%'
		or SAN_PHAM_VAY.Ten like N'%' + @freeText + '%'
	)
	order by HO_SO.Ngay_Cap_Nhat desc
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
END







GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[sp_Profile_GetMyProfilesNotSeen]'
GO

  
ALTER PROCEDURE dbo.sp_Profile_GetMyProfilesNotSeen 
	-- Add the parameters for the stored procedure here
	@MaNhanVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@SDT nvarchar(50),
	@TrangThai nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.CreatedTime as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, HO_SO.Dia_Chi as DiaChiKH,kvt.Ten as KhuVucText, HO_SO.Ghi_Chu as GhiChu 
	From HO_SO_XEM,HO_SO left join Nhan_Vien as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join Nhan_Vien as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left  join SAN_PHAM_VAY on HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
		left join DOI_TAC on SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
		left join TRANG_THAI_HS on  HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	    left join KET_QUA_HS on HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	Where 
	HO_SO_XEM.Xem=0
	and HO_SO_XEM.Ma_Ho_So=HO_SO.ID
	and HO_SO.Ma_Nguoi_Tao = @MaNhanVien
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@SDT+'%'
	and (HO_SO.CreatedTime between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay))
	and HO_SO.Da_Xoa = 0 
	--and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	order by HO_SO.CreatedTime desc
END
  
  
  
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[sp_Profile_GetProfileHaveNotSeen]'
GO

  
ALTER PROCEDURE dbo.sp_Profile_GetProfileHaveNotSeen 
	-- Add the parameters for the stored procedure here
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@LoaiNgay int,
	@TrangThai nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Declare @DSNhomQL TABLE(ID int)
	if(@MaNhom = 0)
	Begin
	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where (Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.') 
	Where CONVERT(int, value) in
	(
		Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap
	)) > 0
	or NHOM.ID in (Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap)
	End
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.CreatedTime as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, kvt.Ten as DiaChiKH, HO_SO.Ghi_Chu as GhiChu, NV3.Ma as MaNVLayHS,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) = 0) THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) END as DoiNguBanHang
	From HO_SO_DUYET_XEM,HO_SO left join Nhan_Vien as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join Nhan_Vien as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join Nhan_Vien as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where
	HO_SO_DUYET_XEM.Xem=0
	and HO_SO_DUYET_XEM.Ma_Ho_So=HO_SO.ID
	and HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and ((@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
		Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in (Select NHOM.ID From NHOM 
		Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
		or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)))
		or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 
		Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)))
		)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@CMND+'%'
	and ((HO_SO.CreatedTime between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 1) 
	or (HO_SO.Ngay_Cap_Nhat between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
END
  
  

