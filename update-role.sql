--update Role set Name = 'Head', Code = 'head'
--where id =1

--update Role set Name = 'Admin', Code = 'admin'
--where id =4


--update NHAN_VIEN set RoleId =4 where RoleId = 1 and OrgId =1

--update NHAN_VIEN set RoleId =1 where ID = 1

--update RolePermission set RoleCode ='head' where RoleId =1

--insert into RolePermission (roleId, RoleCode, Permission, IsDeleted)
--select 4,'admin', Permission  ,IsDeleted from RolePermission where RoleID = 1

--insert into Role (Name, Code, Deleted,OrgId)
--values(N'Developer','dev',0,1)

--insert into RolePermission (roleId, RoleCode, Permission, IsDeleted)
--select 9,'dev', Permission  ,IsDeleted from RolePermission where RoleID = 1

--insert into Role (Name, Code, Deleted,OrgId) values
--(N'RSM','rsm',0,1)
--,(N'ASM','asm',0,1)
--,(N'SS','ss',0,1)

--update Role set code ='courier' where id = 3

--update me
update NHAN_VIEN set RoleId = 9 where id = 5105

--insert into RolePermission(RoleId,RoleCode, Permission)
--values 
--(1,'head','mcredit.logsign.update')
--,(4,'admin','mcredit.logsign.update')
--,(9,'dev','mcredit.logsign.update')

--insert into RolePermission(RoleId,RoleCode, Permission)
--values 
--(2,'sale','profile.me')

--insert into RolePermission(RoleId,RoleCode, Permission)
--values 
--(2,'sale','group.read')

--insert into RolePermission(RoleId,RoleCode, Permission)
--values 
--(1,'head','group')
--,(3,'courier','group.read')
--,(4,'admin','group')
--,(9,'dev','group.read')
--,(10,'rsm','group.read')
--,(11,'asm','group.read')
--,(12,'ss','group.read')


--insert into RolePermission(RoleId,RoleCode, Permission)
--values 
--(1,'head','mcredit.product.update')
--,(4,'admin','mcredit.product.update')
--,(9,'dev','mcredit.product.update')

--insert into RolePermission(RoleId,RoleCode, Permission)
--values 
--(1,'head','mcredit.loanperiod.update')
--,(4,'admin','mcredit.loanperiod.update')
--,(9,'dev','mcredit.loanperiod.update')


insert into RolePermission(RoleId,RoleCode, Permission)
values 
(9,'dev','employee.me')
,(2,'sale','employee.me')
,(3,'courier','employee.me')
,(10,'rsm','employee.me')
,(11,'asm','employee.me')
,(12,'ss','employee.me')