using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Courier
{
    public class CourierUpdateModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Cmnd { get; set; }
        public string ReceiveDate { get; set; }
        public string SaleCode { get; set; }
        public int Status { get; set; }
        public string LastNote { get; set; }
        public int AssignId { get; set; }
        public int GroupId { get; set; }
        public int DistrictId { get; set; }
        public int ProvinceId { get; set; }
    }
}
