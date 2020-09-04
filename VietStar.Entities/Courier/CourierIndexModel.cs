using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities.Commons;

namespace VietStar.Entities.Courier
{
    public class CourierIndexModel : Pagination
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Cmnd { get; set; }
        public int Status { get; set; }
        public int GroupId { get; set; }
        public string LastNote { get; set; }
        public int AssignId { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public string SaleCode { get; set; }
        public List<int> AssigneeIds { get; set; }
        public string AssignUser { get; set; }
        public string ProductName { get; set; }
        public string CreatedUser { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int UpdatedBy { get; set; }
    }
}
