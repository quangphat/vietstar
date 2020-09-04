USE [PortalPro]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetHosoCourier]    Script Date: 05/09/2020 12:19:43 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_GetHosoCourier]
(
@freeText nvarchar(30),
@assigneeId int = 0,
@status varchar(20) ='',
@provinceId int = 0,
@page int =1,
@limit_tmp int = 10,
@groupId int =0,
@saleCode varchar(20) ='',
@userId int
)
as
begin
declare @where  nvarchar(1000) = '';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
if @freeText = '' begin set @freeText = null end;
if @saleCode = '' begin set @saleCode = null end;
declare @offset int = 0;
set @offset = (@page-1)*@limit_tmp;
set @mainClause = 'select count(*) over() as TotalRecord, hc.Id,
 hc.CustomerName,hc.Cmnd,hc.Status,hc.SaleCode, hc.Phone, hc.AssignUserId, hc.CreatedBy, hc.UpdatedBy,
 hc.CreatedTime, hc.UpdatedTime,fintechcom_vn_PortalNew.fn_getGhichuByHosoId(hc.Id,2) as LastNote, nv2.Ho_Ten as AssignUser,
  nv1.Ho_Ten as CreatedUser,
  ISNULL(kv1.Ten,'''') as ProvinceName, isnull(kv2.Ten,'''') as DistrictName
  from HosoCourrier hc left join Nhan_Vien nv1 on hc.CreatedBy = nv1.ID
  left join Nhan_Vien nv2 on hc.AssignUserId = nv2.ID 
  left join Khu_vuc kv1 on hc.ProvinceId = kv1.Id
  left join Khu_vuc kv2 on hc.DistrictId = kv2.Id'
  if(@freeText  is not null)
  begin
	set @where = ' (hc.CustomerName like  N''%' + @freeText +'%''';
	 set @where = @where + ' or hc.cmnd like  N''%' + @freeText +'%''';
	  set @where = @where + ' or hc.phone like  N''%' + @freeText +'%''';
	   set @where = @where + ' or nv2.Ho_Ten like  N''%' + @freeText +'%'' )';
	   end;
	  
	   if(@where <> '')
	   set @where = @where + ' and';
	   set @where = @where + ' (@userId in (select * from fn_GetUserIDCanViewMyProfile (hc.CreatedBy)) 
	   or (hc.Id in (select CourierId from CourierAssignee where @userId = AssigneeId))
	   or (@userId in (select * from fn_MaNguoiQuanlyByHoSocourierId (hc.Id)))
	   or (hc.SaleCode = (select Ma from Nhan_Vien where id = @userId))'
	    if(@assigneeId > 0)
	   begin
		set @where += ' or hc.SaleCode = (select Ma from Nhan_Vien where Id = @assigneeId))'; 
	   end;
	   else
	   begin
		set @where += ')'
	   end
	   if(@status <> '')
	   begin
	   if(@where <> '')
	   set @where = @where + ' and';
	   set @where = @where + ' hc.Status in ('+ @status +')'; 
	   end;
	   if(@groupId <> 0)
	   begin
	   if(@where <> '')
	   set @where = @where + ' and';
	   set @where = @where + ' hc.GroupId = @groupId';
	   end;
	   if(@provinceId <> 0)
	   begin
	   if(@where <> '')
	   set @where = @where + ' and';
	   set @where = @where + ' hc.ProvinceId = @provinceId'; 
	   end;
	   if(@saleCode is not null)
	   begin
	   if(@where <> '')
	   set @where = @where + ' and';
	   set @where = @where + ' hc.SaleCode = @saleCode'; 
	   end;
	   if(@where <>'')
	   begin
	   set @where= ' where ' + @where
	   end
	   set @where = @where + ' and isnull(hc.IsDeleted,0) = 0  order by hc.createdTime desc'; 
	   set @where += ' offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
	   set @mainClause = @mainClause +  @where
	   set @params =N'@assigneeId  int,@status varchar(20), @offset int, @limit int, @groupId int, @provinceId int
	   , @saleCode varchar(20),@userId int';
	   EXECUTE sp_executesql @mainClause,@params, @assigneeId = @assigneeId, @provinceId = @provinceId,
	   @status = @status, @offset = @offset, @limit = @limit_tmp, @groupId = @groupId, @saleCode = @saleCode,@userId = @userId;
	   print @mainClause;
	   --print @where
	   end



