using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class GetFileUploadResponse : MCResponseModelBase
    {
        public List<GetFileUploadGroup> Groups { get; set; }
        public List<GetFileUploadImage> Images { get; set; }
    }
    public class GetFileUploadGroup
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool Mandatory { get; set; }
        public bool HasAlternate { get; set; }
        public List<object> AlternateGroups { get; set; }
        public List<GetFileUploadDocument> Documents { get; set; }
    }
    public class GetFileUploadDocument
    {
        public int Id { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentName { get; set; }
        public string MapBpmVar { get; set; }
    }
    public class GetFileUploadImage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string GroupId { get; set; }
        public string SrvDocCode { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
    }
}
