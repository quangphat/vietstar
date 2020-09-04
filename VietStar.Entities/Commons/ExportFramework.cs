using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Commons
{
    public class ExportFramework
    {
        public int Id { get; set; }
        public string ColPosition { get; set; }
        public string FieldName { get; set; }
        public string ProfileType { get; set; }
        public int OrgId { get; set; }
    }
}
