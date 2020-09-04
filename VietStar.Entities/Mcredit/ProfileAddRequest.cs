using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities.Mcredit;

namespace VietStar.Entities.Mcredit
{
    public class ProfileAddRequest : MCreditRequestModelBase
    {
        public MCProfilePostModel Obj { get; set; }
    }
    public class ProfileAddObj
    {
        public string name { get; set; }
        public string homeTown { get; set; }
        public string cityId { get; set; }
        public string bod { get; set; }
        public string productCode { get; set; }
        public string phone { get; set; }
        public string idNumber { get; set; }
        public string cccdNumber { get; set; }
        public string idNumberDate { get; set; }
        public bool isAddr { get; set; }
        public string loanPeriodCode { get; set; }
        public string loanMoney { get; set; }
        public bool isInsurance { get; set; }
        public string logSignCode { get; set; }
        public string saleID { get; set; }
        public int status { get; set; }
    }
}
