using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.CheckDup
{
    public class CheckDupNote : SqlBaseModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Note { get; set; }
    }
}
