--USE [PortalPro]
--GO
--/****** Object:  StoredProcedure [dbo].[sp_HO_SO_LayChiTiet]    Script Date: 04/09/2020 11:58:27 CH ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--ALTER PROCEDURE [dbo].[sp_HO_SO_LayChiTiet]
--	-- Add the parameters for the stored procedure here
--	@ID int
--AS
--BEGIN
--	-- SET NOCOUNT ON added to prevent extra result sets from
--	-- interfering with SELECT statements.
--	select hs.ID
--	,Ma_Ho_So as MaHoSo
--	,Ten_Khach_Hang as TenKhachHang
--	,CMND,Dia_Chi as DiaChi
--	,Courier_Code as CourierCode,
--	SDT,SDT2
--	,Gioi_Tinh as GioiTinh
--	,hs.CreatedTime as NgayTao
--	,hs.CreatedBy as MaNguoiTao,
--	hs.DisbursementDate,
--	Ma_Khu_Vuc as MaKhuVuc
--	, tt.Ten as TenTrangThai
--	, kq.Ten as KetQuaText,
--	AssigneeId as HoSoCuaAi
--	,hs.UpdatedTime as NgayCapNhat
--	,hs.UpdatedBy as MaNguoiCapNhat,
--	Ngay_Nhan_Don as NgayNhanDon
--	,Ma_Trang_Thai as MaTrangThai
--	,Ma_Ket_Qua as MaKetQua,San_Pham_Vay as SanPhamVay
--	,sv.Ten as ProductName
--	,Ten_Cua_Hang as TenCuaHang
--	,Co_Bao_Hiem as CoBaoHiem,So_Tien_Vay as SoTienVay,Han_Vay as HanVay,Ghi_Chu as GhiChu
--	,BirthDay,CmndDay,
--	dbo.getDoitacBySanphamId(sv.ID,0) as Doitac,
--	dbo.getDoitacBySanphamId(sv.ID,1) as PartnerId,
--	sv.Ten as Sanpham,
--	sv.Ma as ProductCode
--	,dbo.getProvinceNameFromDistrictId(hs.Ma_Khu_Vuc) as ProvinceName
--	,dbo.getProvinceIdFromDistrictId(hs.Ma_Khu_Vuc) as ProvinceId
--	,kv.Ten as DistrictName,
--	nv.Ten_Dang_Nhap as SaleCode,
--	nv.Ho_Ten as EmployeeName,
--	nv.Dien_Thoai as EmployeePhone,
--	 fintechcom_vn_PortalNew.fn_getGhichuByHosoId(hs.ID,1) as GhiChu
--	from HO_SO hs
--	left join TRANG_THAI_HS tt on tt.ID=hs.Ma_Trang_Thai
--	left join KET_QUA_HS kq on kq.ID=hs.Ma_Ket_Qua
--	left join SAN_PHAM_VAY sv on hs.San_Pham_Vay = sv.ID
--	left join KHU_VUC kv on hs.Ma_Khu_Vuc = kv.ID
--	left join NHAN_VIEN nv on hs.AssigneeId = nv.ID
--	 where hs.ID=@ID
--END



