USE [PortalPro]
GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_Count_TimHoSoQuanLy]    Script Date: 04/09/2020 11:55:48 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_HO_SO_Count_TimHoSoQuanLy] 
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
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.CreatedBy = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.UpdatedBy = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and (
			(@MaThanhVien > 0 and HO_SO.CreatedBy = @MaThanhVien) 
			or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.CreatedBy in (
			Select NHAN_VIEN_NHOM.Ma_Nhan_Vien 
			From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in 
			(Select NHOM.ID From NHOM Where 
			((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
			or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)
			))
			or (@MaNhom = 0 and HO_SO.CreatedBy in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 
			Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)) and @MaThanhVien = 0)
	)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and (HO_SO.Ma_Ho_So like '%'+@MaHS+'%')
	and (HO_SO.CMND like '%'+@CMND+'%')
	and ((convert(varchar(10), HO_SO.CreatedTime,121) between CONVERT(varchar(10), @TuNgay,121) and CONVERT(varchar(10), @DenNgay,121) and @LoaiNgay = 1) 
	or ( convert(varchar(10),HO_SO.UpdatedTime,121) between CONVERT(varchar(10), @TuNgay,121) and CONVERT(varchar(10), @DenNgay,121) and @LoaiNgay = 2))
	and HO_SO.IsDeleted = 0
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

