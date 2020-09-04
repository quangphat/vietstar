using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class AuthenRequestModel : MCreditRequestModelBase
    {
        public string UserName { get; set; }
        public string UserPass { get; set; }
    }
}
