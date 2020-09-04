USE [PortalPro]
GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoDuyetChuaXem]    Script Date: 05/09/2020 12:02:36 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
--GO



---- =============================================
---- Author:		<Author,,Name>
---- Create date: <Create Date,,>
---- Description:	<Description,,>
---- =============================================
--ALTER PROCEDURE [dbo].[sp_HO_SO_TimHoSoDuyetChuaXem] 
--	-- Add the parameters for the stored procedure here
--	@MaNVDangNhap int,
--	@MaNhom int,
--	@MaThanhVien int,
--	@TuNgay datetime,
--	@DenNgay datetime,
--	@MaHS nvarchar(50),
--	@CMND nvarchar(50),
--	@LoaiNgay int,
--	@TrangThai nvarchar(50)
--AS
--BEGIN
--	-- SET NOCOUNT ON added to prevent extra result sets from
--	-- interfering with SELECT statements.
--	Declare @DSNhomQL TABLE(ID int)
--	if(@MaNhom = 0)
--	Begin
--	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where (Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.') Where CONVERT(int, value) in
--	(
--		Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap
--	)) > 0
--	or NHOM.ID in (Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap)
--	End
--	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.CreatedTime as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, HO_SO.UpdatedTime as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, kvt.Ten as DiaChiKH, HO_SO.Ghi_Chu as GhiChu, NV3.Ma as MaNVLayHS,
--	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.CreatedBy) = 0) THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.CreatedBy) END as DoiNguBanHang
--	From HO_SO_DUYET_XEM,HO_SO left join NHAN_VIEN as NV1 on HO_SO.CreatedBy = NV1.ID
--		left join NHAN_VIEN as NV2 on HO_SO.UpdatedBy = NV2.ID
--		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
--		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
--		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
--	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
--	Where
--	HO_SO_DUYET_XEM.Xem=0
--	and HO_SO_DUYET_XEM.Ma_Ho_So=HO_SO.ID
--	and HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
--	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
--	and ((@MaThanhVien > 0 and HO_SO.CreatedBy = @MaThanhVien) or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.CreatedBy in (
--		Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in (Select NHOM.ID From NHOM Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)))
--		or (@MaNhom = 0 and HO_SO.CreatedBy in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)))
--		)
--	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
--	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
--	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
--	and HO_SO.SDT like '%'+@CMND+'%'
--	and ((HO_SO.CreatedTime between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 1)
--	OR (HO_SO.UpdatedTime between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
--	and HO_SO.IsDeleted = 0
--	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
--END




GO

