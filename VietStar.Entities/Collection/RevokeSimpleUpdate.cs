using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Collection
{
    public class RevokeSimpleUpdate
    {
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int Status { get; set; }
        public int AssigneeId { get; set; }
        public int GroupId { get; set; }
    }
}
