SELECT * FROM dbo.Nhan_Vien WHERE Xoa = 0

SELECT * FROM dbo.Nhan_Vien WHERE xoa <> 0 AND Xoa IS NOT NULL

UPDATE dbo.NHAN_VIEN SET Xoa = 0 WHERE ISNULL(Xoa,0) = 0

UPDATE dbo.NHAN_VIEN SET Xoa = 1 WHERE xoa <> 0 AND Xoa IS NOT NULL

ALTER TABLE dbo.NHAN_VIEN 
ALTER COLUMN Xoa BIT

EXEC sp_rename '[Nhan_Vien].Xoa', 'IsDeleted', 'COLUMN'
---------------
go
 ALTER procedure [dbo].[sp_Employee_GetByCode](@code varchar(20), @userId int = 0)
as
begin
  declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
select top 1 Id, Ma as Code from Nhan_Vien where Ma = @code and isnull(IsDeleted,0) = 0 and isnull(OrgId,0) = @orgId
end
  
-------------
GO
  ALTER procedure [dbo].[sp_Employee_GetByUsername](@userId int, @username varchar(40))
  as
  begin
  declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
  select * from Nhan_Vien where isnull(IsDeleted,0) = 0 and isnull(OrgId,0) = @orgId and Ten_Dang_Nhap = @username
  END
  
  -------
  ALTER PROCEDURE [dbo].[sp_Employee_GetFull] 
(@orgId int =0)
AS
BEGIN
Select  Id, Ma + ' - ' + Ho_Ten as Name From Nhan_Vien where isnull(OrgId,0) = @orgId and isnull(IsDeleted,0) = 0 
 order by ID desc
END

-------------

ALTER procedure [dbo].[sp_Employee_InsertUser_v2]
(
@id int out,
@userName varchar(50),
@code varchar(50),
@password varchar(50),
@fullName nvarchar(100),
@phone varchar(50),
@email varchar(50),
@roleId int,
@createdby int
)
as
begin
declare @orgId int  = 0;
select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @createdby;
insert into Nhan_Vien(Ma,Ten_Dang_Nhap,Mat_Khau,Ho_Ten,Dien_Thoai,Email,RoleId,Status,IsDeLeted,CreatedTime,CreatedBy, OrgId, UpdatedTime)
values (@code,@userName,@password,@fullName,@phone,@email,@roleId,1,0,GETDATE(),@createdby, @orgId, GETDATE());
SET @id=@@IDENTITY
END
--------------


GO
ALTER PROCEDURE [dbo].[sp_SAN_PHAM_VAY_LayDSThongTinByID]
	-- Add the parameters for the stored procedure here
	@MaDoiTac int,
	@NgayTao datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select SAN_PHAM_VAY.ID, SAN_PHAM_VAY.Ten, SAN_PHAM_VAY.Ma, SAN_PHAM_VAY.Ngay_Tao as NgayTao, NHAN_VIEN.Ho_Ten as NguoiTao
	from SAN_PHAM_VAY left join NHAN_VIEN on NHAN_VIEN.ID = SAN_PHAM_VAY.Ma_Nguoi_Tao
	where Ma_Doi_Tac=@MaDoiTac and SAN_PHAM_VAY.Ngay_Tao = @NgayTao
	and isnull(SAN_PHAM_VAY.IsDeleted,0) <> 1
END


----------


GO

ALTER PROCEDURE [dbo].[sp_Employee_Login]
	@UserName nvarchar(50),
	@Password nvarchar(200)
AS
BEGIN
declare @roleId int = 1;
select @roleId = isnull(RoleId, 0) from  Nhan_Vien where Ten_Dang_Nhap=@UserName and Mat_Khau=@Password and isnull(IsDeleted,0) =0
if(@roleId =5)
set @roleId = 1;
	Select Id,Ten_Dang_Nhap AS UserName, Mat_Khau as Passowrd, Ma as Code,
	Email, Ho_Ten as FullName, Dien_Thoai as Phone, Status as IsActive, isnull(OrgId,0) as OrgId, @roleId as RoleId
	 From Nhan_Vien where Ten_Dang_Nhap=@UserName and Mat_Khau=@Password and isnull(IsDeleted,0) =0
END

----------

GO

 ALTER procedure [dbo].[sp_GetEmployees]
(
@workFromDate datetime,
@workToDate datetime,
@freeText nvarchar(30),
@roleId int,
@page int,
@limit int,
@OrgId int =0,
@currentUserId int  
)
as
begin
if @freeText = '' begin set @freeText = null end;
declare @offset as int;
set @offset = (@page-1)*@limit;
Select count(*) over() as TotalRecord, n.ID,n.Ten_Dang_Nhap as UserName,n.Ho_Ten as FullName,n.RoleId,r.Name as RoleName,
n.Email, n.Dien_Thoai as Phone,n.CreatedTime,
n.Ma as Code,
n.WorkDate,CONCAT(k.Ten + ' - ',k2.Ten) as Location
 from Nhan_Vien n left join KHU_VUC k on n.DistrictId = k.ID
 left join KHU_VUC k2 on k.Ma_Cha = k2.ID
 left join Role r on n.RoleId = r.Id
where 
--( convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@workFromDate,121))
--and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@workToDate,121)) or n.WorkDate is null) and 
(@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'
		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'
		or n.Dien_Thoai like N'%' + @freeText + '%'
		or n.Email like N'%' + @freeText + '%')
		and ((@roleId <> 0 and n.RoleId = @roleId) or (@roleId = 0 and (n.RoleId <> 0 or n.RoleId is null)))
		and n.IsDeleted = 0
		and n.ID  != @currentUserId
		and isnull(n.OrgId,0) = @OrgId
order by n.Id desc 
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
end


---------------

GO
ALTER procedure [dbo].[sp_GetNhanvien]
(
@workFromDate datetime,
@workToDate datetime,
@freeText nvarchar(30),
@roleId int,
@page int,
@limit int
)
as
begin
if @freeText = '' begin set @freeText = null end;
declare @offset as int;
set @offset = (@page-1)*@limit;
Select n.ID,n.Ten_Dang_Nhap as UserName,n.Ho_Ten as FullName,n.RoleId,r.Name as RoleName,
n.Email, n.Dien_Thoai as Phone,n.CreatedTime,
n.Ma as Code,
n.WorkDate,CONCAT(k.Ten + ' - ',k2.Ten) as Location
 from NHAN_VIEN n left join KHU_VUC k on n.DistrictId = k.ID
 left join KHU_VUC k2 on k.Ma_Cha = k2.ID
 left join Role r on n.RoleId = r.Id
where ( convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@workFromDate,121))
and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@workToDate,121)) or n.WorkDate is null)
	and (@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'
		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'
		or n.Dien_Thoai like N'%' + @freeText + '%'
		or n.Email like N'%' + @freeText + '%')
		and ((@roleId <> 0 and n.RoleId = @roleId) or (@roleId = 0 and (n.RoleId <> 0 or n.RoleId is null)))
		and n.IsDeleted = 0
order by n.Id desc 
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
end


-------------

GO

  
ALTER procedure [dbo].[sp_CountEmployee]

(

@workFromDate datetime,

@workToDate datetime,

@roleId int,

@freeText nvarchar(30)

)

as

begin

if @freeText = '' begin set @freeText = null end;

Select count(*) from Nhan_Vien n

where (convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@workFromDate,121))

and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@workToDate,121)) or n.WorkDate is null)

	and (@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'

		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'

		or n.Dien_Thoai like N'%' + @freeText + '%'

		or n.Email like N'%' + @freeText + '%')

		and ((@roleId <> 0 and n.RoleId = @roleId) or (@roleId = 0 and (n.RoleId <> 0 or n.RoleId is null)))

		and n.IsDeleted = 0

end
  

  --------------

  GO
--select * from NHAN_VIEN



ALTER procedure [dbo].[sp_CountNhanvien]

(

@workFromDate datetime,

@workToDate datetime,

@roleId int,

@freeText nvarchar(30)

)

as

begin

if @freeText = '' begin set @freeText = null end;

Select count(*) from NHAN_VIEN n

where (convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@workFromDate,121))

and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@workToDate,121)) or n.WorkDate is null)

	and (@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'

		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'

		or n.Dien_Thoai like N'%' + @freeText + '%'

		or n.Email like N'%' + @freeText + '%')

		and ((@roleId <> 0 and n.RoleId = @roleId) or (@roleId = 0 and (n.RoleId <> 0 or n.RoleId is null)))

		and n.IsDeleted = 0

end




------------------

GO

  
ALTER procedure [dbo].[sp_Employee_Delete_v2]
(
@id int,
@updatedby int,
@DeletedBy int, 
@Xoa bit
)
as
begin
--declare @oldRoleId int = 0;
--select @oldRoleId = RoleId from NHAN_VIEN where ID = @id;
--if(@oldRoleId is not null and @oldRoleId >0 )
--begin

--end
update Nhan_Vien set 
		
		UpdatedBy = @updatedby,
		DeletedBy  = @DeletedBy,
		IsDeleted  = @Xoa
		where ID = @id
END

--------
