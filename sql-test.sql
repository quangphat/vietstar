--- can delete
sp_CountCustomer
sp_CountEmployee
sp_CountHosoCourier
sp_CountNhanvien
sp_GetAllMenu
sp_getCheckDupNotesById
sp_GetCompnay
sp_MCredit_TempProfile_Counts
---mcredit
  exec sp_MCredit_TempProfile_Gets 'fffffddddddd', 1, '',1,10,0,0,'2020-08-05', '2020-09-05', 1
  exec sp_MCredit_TempProfile_Gets '',1,'', 1,10,0,0,'2020-07-01','2020-08-1',1

  --- nhan_vien
  exec sp_Employee_GetPaging 0, '',1,10

  -----customer-checkdup

  exec sp_GetCustomer '', 0, 10,1
  exec sp_GetCustomer_v2 '', 0,10,1

  ------courier

  exec sp_GetHosoCourier '', 0,'', 0, 1, 10, 0, '', 1


  ------revoke
  exec sp_RevokeDebt_Search '','',1,10,0,0,1,'2020-07-01', '2020-09-01',1,0,0
