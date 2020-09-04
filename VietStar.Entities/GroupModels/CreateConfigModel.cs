using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.GroupModels
{
    public class CreateConfigModel
    {
        public int UserId { get; set; }
        public List<int> GroupIds { get; set; }
    }
}
