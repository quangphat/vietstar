update Role set Name = 'Head', Code = 'head'
where id =1

update Role set Name = 'Admin', Code = 'admin'
where id =4


update NHAN_VIEN set RoleId =4 where RoleId = 1 and OrgId =1

update NHAN_VIEN set RoleId =1 where ID = 1

update RolePermission set RoleCode ='head' where RoleId =1

insert into RolePermission (roleId, RoleCode, Permission, IsDeleted)
select 4,'admin', Permission  ,IsDeleted from RolePermission where RoleID = 1

insert into Role (Name, Code, Deleted,OrgId)
values(N'Developer','dev',0,1)

insert into RolePermission (roleId, RoleCode, Permission, IsDeleted)
select 9,'dev', Permission  ,IsDeleted from RolePermission where RoleID = 1

insert into Role (Name, Code, Deleted,OrgId) values
(N'RSM','rsm',0,1)
,(N'ASM','asm',0,1)
,(N'SS','ss',0,1)

update Role set code ='courier' where id = 3

--update me
update NHAN_VIEN set RoleId = 9 where id = 5105

insert into RolePermission(RoleId,RoleCode, Permission)
values 
(1,'head','mcredit.logsign.update')
,(4,'admin','mcredit.logsign.update')
,(9,'dev','mcredit.logsign.update')