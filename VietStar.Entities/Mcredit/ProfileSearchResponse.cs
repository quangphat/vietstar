using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class ProfileSearchResponse : MCResponseModelBase
    {
        public string token { get; set; }
        public int totaler { get; set; }
        public int totalItem { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public List<ProfileSearchResponseItem> objs { get; set; }
    }
    public class ProfileSearchResponseItem
    {
        public string id { get; set; }
        public string caseNumber { get; set; }
        public string name { get; set; }
        //public string cccdNumber { get; set; }
        public string idNumber { get; set; }
        public string moneyReceiveDate { get; set; }
        public string status { get; set; }
        public string statusName { get; set; }
        public string createUserId { get; set; }
        public string createUserCode { get; set; }
        public string creeateUserName { get; set; }
        public string createUserFullName { get; set; }
        public string createDate { get; set; }
    }
}
