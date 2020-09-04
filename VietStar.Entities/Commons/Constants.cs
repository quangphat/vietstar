using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Commons
{
    public class Constants
    {
        public const int Limit_Max_Page = 100;
        public const int PasswordMinLengthRequire = 6;
        public const int MaxFileSize = 20;// MB
        public const string DownloadFolder = "Download";
        public const string ReportTemplate = "TemplateReport";
        public const string ExportDanhsachHosoBaseFileName = "Report-DSHS";
        public const string CurrentCyFormat = "{0:#,###.##}";
        public const string revoke_debt_max_row_import = "revoke_debt_max_row_import";
        public const string revoke_debt_profile_status_vp_field = "revoke_Field";
        public const string revoke_debt_profile_status_vp_call = "revoke_Call";
    }
}
