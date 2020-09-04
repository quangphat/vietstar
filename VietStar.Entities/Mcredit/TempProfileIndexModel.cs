using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities.Commons;

namespace VietStar.Entities.Mcredit
{
    public class TempProfileIndexModel : Pagination
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string IdNumber { get; set; }
        public string MoneyReceiveDate { get; set; }
        public string McId { get; set; }
        public int Status { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
        public string LastNote { get; set; }
        public string SaleName { get; set; }
        public string ProductName { get; set; }
        public string LoanPeriodName { get; set; }
        public string Phone { get; set; }
        public int TotalRecord { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int UpdatedBy { get; set; }
    }
}
