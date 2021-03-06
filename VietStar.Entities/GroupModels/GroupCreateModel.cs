﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.GroupModels
{
    public class GroupCreateModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string LeaderId { get; set; }
        public int ParentId { get; set; }
        public List<int> MemberIds { get; set; }
    }
}
