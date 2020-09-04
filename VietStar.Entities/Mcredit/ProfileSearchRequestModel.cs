using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class ProfileSearchRequestModel : MCreditRequestModelBase
    {
        public string str { get; set; }
        public string status { get; set; }
        public int page { get; set; }
        public string type { get; set; }
    }
}
