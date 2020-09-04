using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class CheckCatResponseModel : MCResponseModelBase
    {
        public string name { get; set; }
        public string cat { get; set; }
        public string addr { get; set; }
        public string phone { get; set; }
    }
}
