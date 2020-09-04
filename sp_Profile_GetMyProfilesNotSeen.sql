USE [PortalPro]
GO
/****** Object:  StoredProcedure [dbo].[sp_Profile_GetMyProfilesNotSeen]    Script Date: 04/09/2020 11:50:14 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  
ALTER PROCEDURE [dbo].[sp_Profile_GetMyProfilesNotSeen] 
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
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.CreatedTime as NgayTao, DOI_TAC.Ten as DoiTac,
	HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, 
	TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, HO_SO.UpdatedTime as NgayCapNhat, 
	NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, 
	HO_SO.Dia_Chi as DiaChiKH,kvt.Ten as KhuVucText, HO_SO.Ghi_Chu as GhiChu 
	From HO_SO_XEM,HO_SO left join Nhan_Vien as NV1 on HO_SO.CreatedBy = NV1.ID
		left join Nhan_Vien as NV2 on HO_SO.UpdatedBy = NV2.ID
		left  join SAN_PHAM_VAY on HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
		left join DOI_TAC on SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
		left join TRANG_THAI_HS on  HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	    left join KET_QUA_HS on HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	Where 
	HO_SO_XEM.Xem=0
	and HO_SO_XEM.Ma_Ho_So=HO_SO.ID
	and HO_SO.CreatedBy = @MaNhanVien
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@SDT+'%'
	and (HO_SO.CreatedTime between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay))
	and HO_SO.IsDeleted = 0 
	--and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	order by HO_SO.CreatedTime desc
END
  
  
  
