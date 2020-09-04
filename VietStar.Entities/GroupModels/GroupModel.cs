using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.GroupModels
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string ParentCode { get; set; }
        public string Name { get; set; }
        public string ParentSequenceCode { get; set; }
        public string ShortName { get; set; }
        public int LeaderId { get; set; }
        public string LeaderName { get; set; }
    }
}
