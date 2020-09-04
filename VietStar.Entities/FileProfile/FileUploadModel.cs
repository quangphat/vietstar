using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.FileProfile
{
    public class FileUploadModel
    {
        public int Key { get; set; }
        public string KeyName { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int FileId { get; set; }
        public string Id { get; set; }
        public bool? IsRequire { get; set; }
        public int ProfileId { get; set; }
        public int ProfileTypeId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentCode { get; set; }
        public int MC_DocumentId { get; set; }
        public string MC_MapBpmVar { get; set; }
        public int MC_GroupId { get; set; }
        public string FullPath { get; set; }
        public string Folder { get; set; }
        public string GuidId { get; set; }
    }
}
