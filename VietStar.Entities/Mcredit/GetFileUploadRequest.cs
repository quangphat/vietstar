using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class GetFileUploadRequest : MCreditRequestModelBase
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Loccode { get; set; }
        public string Issl { get; set; }
        public string Money { get; set; }
    }
}
