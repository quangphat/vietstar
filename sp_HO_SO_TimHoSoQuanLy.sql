USE [PortalPro]
GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoQuanLy]    Script Date: 04/09/2020 11:53:28 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
--GO

--  ALTER PROCEDURE [dbo].[sp_HO_SO_TimHoSoQuanLy] 
--	-- Add the parameters for the stored procedure here
--	@offset int,
--	@limit int,
--	@MaNVDangNhap int,
--	@MaNhom int,
--	@MaThanhVien int,
--	@TuNgay datetime,
--	@DenNgay datetime,
--	@MaHS nvarchar(50),
--	@CMND nvarchar(50),
--	@TrangThai nvarchar(50),
--	@freeText as nvarchar(50) = '',
--	@LoaiNgay int
--AS
--BEGIN
--	-- SET NOCOUNT ON added to prevent extra result sets from
--	-- interfering with SELECT statements.
--	if @freeText = '' begin set @freeText = null end;
--	if @MaHS is null begin set @MaHS ='' end;
--	if @CMND is null begin set @CMND ='' end;
--	if CHARINDEX(',6',@TrangThai) >0
--	begin
--	 set @TrangThai+=',19'
--	end
--	Declare @DSNhomQL TABLE(ID int)
--	if(@MaNhom = 0)
--	Begin
--	-- Lay nhom la thanh vien
--	Insert Into @DSNhomQL Select NHAN_VIEN_NHOM.Ma_Nhom From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = @MaNVDangNhap
--	-- Lay nhom con truc tiep (nv la nguoi quan ly)
--	Insert Into @DSNhomQL Select NHOM.ID From NHOM Where NHOM.Ma_Nhom_Cha in (select * from @DSNhomQL)
--	-- Lay ds nhom tu nhom quan ly truc tiep tro xuong
--	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where 
--	(Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.') 
--		Where CONVERT(int, value) in (Select * From @DSNhomQL)) > 0
--	End
--	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo,HO_SO.DisbursementDate, HO_SO.CreatedTime as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND,
--	 HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai ,TRANG_THAI_HS.Ten as TrangThaiHS,
--	 KET_QUA_HS.Ten as KetQuaHS, HO_SO.UpdatedTime as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang,
--	  NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, HO_SO.Dia_Chi as DiaChiKH,
--	  isnull(HO_SO.SDT2,'') as Phone,
--	  kvt.Ten as KhuVucText, fintechcom_vn_PortalNew.fn_getGhichuByHosoId(HO_SO.ID,1) as GhiChu, NV3.Ma as MaNVLayHS,
--	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM 
--	Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.CreatedBy) = 0) 
--	THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM 
--	Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.CreatedBy)
--	 END as DoiNguBanHang, 
--	 SAN_PHAM_VAY.Ten as TenSanPham
--	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.CreatedBy = NV1.ID
--		left join NHAN_VIEN as NV2 on HO_SO.UpdatedBy = NV2.ID
--		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
--		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
--		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
--	, DOI_TAC, SAN_PHAM_VAY,TRANG_THAI_HS, KET_QUA_HS
--	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
--	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
--	and (
--			(@MaThanhVien > 0 and HO_SO.CreatedBy = @MaThanhVien) 
--			or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.CreatedBy in (
--			Select NHAN_VIEN_NHOM.Ma_Nhan_Vien 
--			From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in 
--			(Select NHOM.ID From NHOM Where 
--			((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
--			or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)
--			))
--			or (@MaNhom = 0 and HO_SO.CreatedBy in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 
--			Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)) and @MaThanhVien = 0)
--	)
--	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
--	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
--	and (HO_SO.Ma_Ho_So like '%'+@MaHS+'%')
--	and (HO_SO.CMND like '%'+@CMND+'%')
--	and ((convert(varchar(10), HO_SO.CreatedTime,121) between CONVERT(varchar(10), @TuNgay,121) and CONVERT(varchar(10), @DenNgay,121) and @LoaiNgay = 1) 
--	or ( convert(varchar(10),HO_SO.UpdatedTime,121) between CONVERT(varchar(10), @TuNgay,121) and CONVERT(varchar(10), @DenNgay,121) and @LoaiNgay = 2))
--	and HO_SO.IsDeleted = 0
--	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
--	and (@freeText is null or HO_SO.Ma_Ho_So like N'%' + @freeText + '%'
--		or DOI_TAC.Ten like N'%' + @freeText + '%'
--		or HO_SO.CMND like N'%' + @freeText + '%'
--		or HO_SO.SDT like N'%' + @freeText + '%'
--		or HO_SO.Ten_Khach_Hang like N'%' + @freeText + '%'
--		or KET_QUA_HS.Ten like N'%' + @freeText + '%'
--		or NV1.Ma like N'%' + @freeText + '%'
--		or NV1.Ho_Ten like N'%' + @freeText + '%'
--		or NV2.Ma like N'%' + @freeText + '%'
--		or NV3.Ma like N'%' + @freeText + '%'
--		or kvt.Ten like N'%' + @freeText + '%'
--		or SAN_PHAM_VAY.Ten like N'%' + @freeText + '%'
--	)
--	order by HO_SO.UpdatedTime desc
--	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
--END







