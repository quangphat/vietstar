using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Infrastructures
{
    public class SystemConfig
    {
        public int ImportMaxRow { get; set; } = 400;
        public string ExportFolder { get; set; } = "wwwroot\\Export";
        public string ExportTemplate { get; set; } = "wwwroot\\ExportTemplate";
        public string ImportTemplate { get; set; } = "wwwroot\\ImportTemplate";
    }
}
