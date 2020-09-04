using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Profile
{
    public class ProfileDetail:ProfileAddSql
    {
        public int PartnerId { get; set; }
        public int ProvinceId { get; set; }
    }
}
