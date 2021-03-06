USE [fintechcom_vn_PortalNew]
GO
/****** Object:  StoredProcedure [dbo].[sp_Employee_GetPaging_v2]    Script Date: 06/09/2020 12:49:57 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Employee_GetPaging_v2] 
(@orgId int =0, 
@freetext nvarchar(50) ='',
 @page int = 1,
  @limit int= 20,
  @ignoreMemberIdGroupId int =0)
AS
BEGIN
declare @offset int = 0;
set @offset = (@page-1)*@limit;
declare @where  nvarchar(1000) = '';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
if @freetext = '' begin set @freetext = null end;
set @mainClause = 'Select count(*) over() as TotalRecord, Id, Ma + '' - '' + Ho_Ten as Name From Nhan_Vien where isnull(OrgId,0) = @orgId and isnull(IsDeleted,0) = 0 ';
if(@freetext  is not null)
	begin
	set @where +=' and (Ma like  N''%' + @freetext +'%''';
	set @where +=' or Ho_Ten like  N''%' + @freetext +'%'' )';
end;
if(@ignoreMemberIdGroupId >0)
	begin
	set @where +=' and Id not in (select Ma_Nhan_Vien from Nhan_Vien_Nhom where Ma_Nhom = @ignoreMemberIdGroupId)';
end;
set @where += ' order by createdTime desc offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N' @offset int, @limit int, @orgId int, @ignoreMemberIdGroupId int';
EXECUTE sp_executesql @mainClause, @params, @offset = @offset, @limit = @limit,@orgId = @orgId,@ignoreMemberIdGroupId =@ignoreMemberIdGroupId;
print @mainClause;
END
