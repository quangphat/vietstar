
UPDATE dbo.HosoCourrier SET OldStatus = Status


--processing
UPDATE dbo.HosoCourrier SET
Status = 10 WHERE OldStatus =2

--new
UPDATE dbo.HosoCourrier SET
Status = 9 WHERE OldStatus =1

--accept
UPDATE dbo.HosoCourrier SET
Status = 12 WHERE OldStatus =4


--finish
UPDATE dbo.HosoCourrier SET
Status = 14 WHERE OldStatus =6

--Input
UPDATE dbo.HosoCourrier SET
Status = 1 WHERE OldStatus =8

--Thẩm định --expertise
UPDATE dbo.HosoCourrier SET
Status = 2 WHERE OldStatus =9

--Additional
UPDATE dbo.HosoCourrier SET
Status = 10 WHERE OldStatus =4

--ACompared--đã đối chiếu
UPDATE dbo.HosoCourrier SET
Status = 6 WHERE OldStatus =11

--ACompared--đã đối chiếu
UPDATE dbo.HosoCourrier SET
Status = 8 WHERE OldStatus =12