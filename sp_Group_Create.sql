
alter procedure [dbo].[sp_Group_Create] (@Id int out,
 @parentId int, 
 @LeaderId int,
 @ShortName nvarchar(50), 
 @Name nvarchar(200), 
 @ParentSequenceCode nvarchar(100),
 @memberIds varchar(200),
 @createdBy int, 
 @orgId int)
as
begin


 insert into NHOM (Ma_Nhom_Cha,Ma_Nguoi_QL,Ten_Viet_Tat,Ten,Chuoi_Ma_Cha,CreatedTime,CreatedBy, UpdatedTime, OrgId)
 values (@parentId,@LeaderId,@ShortName,@Name,@ParentSequenceCode,GETDATE(),@createdBy, GETDATE(), @orgId)
 SET @Id=@@IDENTITY;

 insert into NHAN_VIEN_NHOM (Ma_Nhom ,Ma_Nhan_Vien)
 select @Id, value from dbo.fn_SplitStringToTable(@memberIds, ',')
 end