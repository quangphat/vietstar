using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class CheckSaleResponseModel : MCResponseModelBase
    {
        public CheckSaleObj obj { get; set; }
    }
    public class CheckSaleObj
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string bod { get; set; }
        public string idNumber { get; set; }
        public string sex { get; set; }
        public string addr { get; set; }
        public string addrNow { get; set; }
        public string email { get; set; }
        public string avatar { get; set; }
        public string info { get; set; }

    }
}
