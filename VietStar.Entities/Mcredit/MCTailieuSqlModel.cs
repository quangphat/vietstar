using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities.FileProfile;

namespace VietStar.Entities.Mcredit
{
    public class MCProfileFileSqlModel: ProfileFileAddSql
    {
        public string DocumentName { get; set; }
        public string DocumentCode { get; set; }
        public int MC_DocumentId { get; set; }
        public string MC_MapBpmVar { get; set; }
        public int MC_GroupId { get; set; }
        public int OrderId { get; set; }
        public string McId { get; set; }
    }
}
