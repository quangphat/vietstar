using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Commons
{
    public class ExportRequestModel: ExportRequestModelBase
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int dateType { get; set; }
        public int groupId { get; set; }
        public int memberId { get; set; }
        public int assigneeId { get; set; }
        public string saleCode { get; set; }
        public string status { get; set; }
        public int provinceId { get; set; }
        public string sort { get; set; }
        public int processStatus { get; set; }
        public string sortField { get; set; }
    }

    public class ExportRequestModelBase
    {
        public int userId { get; set; }
        public string freeText { get; set; }
        public int page { get; set; } = 1;
        public int limit { get; set; } = 20;
    }
}
