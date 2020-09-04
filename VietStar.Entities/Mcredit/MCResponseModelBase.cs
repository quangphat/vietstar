using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class MCResponseModelBase
    {
        public string status { get; set; }
        public int code { get; set; }
        public object msg { get; set; }
        public string message { get; set; }
    }
}
