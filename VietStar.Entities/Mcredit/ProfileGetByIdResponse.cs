using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class ProfileGetByIdResponse : MCResponseModelBase
    {
        public ProfileGetByIdResponseObj obj { get; set; }
    }

    public class ProfileGetByIdResponseObj
    {
        public string Id { get; set; }
        public string CaseNumber { get; set; }
        public string Name { get; set; }
        public string Bod { get; set; }
        public string HomeTown { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string CNameNoMark { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ComName { get; set; }
        public string CatNumber { get; set; }
        public string CatName { get; set; }
        public string CccdNumber { get; set; }
        public string IdNumber { get; set; }
        public string IdDate { get; set; }
        public string Phone { get; set; }
        public string IsAddrSame { get; set; }
        public string LoanPeriodId { get; set; }
        public string LoanPeriodCode { get; set; }
        public string LoanPeriod { get; set; }
        public string LoanMoney { get; set; }
        public string MoneyReceive { get; set; }
        public string MoneyReceiveDate { get; set; }
        public string DisbursementDate { get; set; }
        public string MoneyReceivePeriod { get; set; }
        public string IsInsurrance { get; set; }
        public string LocSignId { get; set; }
        public string LocSignCode { get; set; }
        public string LocSignName { get; set; }
        public string LocSignAddr { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string SaleId { get; set; }
        public string SaleCode { get; set; }
        public string SaleName { get; set; }
        public string SalePhone { get; set; }
        public string SaleIdNumber { get; set; }
        public string Refuse { get; set; }
        public object Reason { get; set; }
        public string ReasonName { get; set; }
        public string CreateUserId { get; set; }
        public string CreateUserCode { get; set; }
        public string CreateUserName { get; set; }
        public string CreateUserFullName { get; set; }
        public string CreateUserPhone { get; set; }
        public string CreateDate { get; set; }
        public string SaleHomeTown { get; set; }
        public string SaleAddr { get; set; }
        public string SaleAvatar { get; set; }
        public int LocalProfileId { get; set; }
    }
}
