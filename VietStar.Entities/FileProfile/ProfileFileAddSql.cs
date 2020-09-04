using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.FileProfile
{
    public class ProfileFileAddSql
    {
        public int FileId { get; set; }
        public int FileKey { get; set; }
        public string FilePath { get; set; }
        public string Folder { get; set; }
        public string FileName { get; set; }
        public int ProfileId { get; set; }
        public int ProfileTypeId { get; set; }
        public string GuidId { get; set; }
    }
}
