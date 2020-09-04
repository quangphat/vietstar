using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.FileProfile
{
    public class FileProfile : FileProfileType
    {
        public List<FileUploadModel> ProfileFiles { get; set; }
    }
}
