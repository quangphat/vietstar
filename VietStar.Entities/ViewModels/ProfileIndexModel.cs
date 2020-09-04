using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities.Commons;

namespace VietStar.Entities.ViewModels
{
    public class ProfileIndexModel : Pagination
    {
        public int Id { get; set; }
        public string ProfileCode { get; set; }
        public string CMND { get; set; }
        public string Phone { get; set; }
        public string StatusName { get; set; }
        public string CustomerName { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }
        public string LastNote { get; set; }
        public string CourierCode { get; set; }
        public string ProductName { get; set; }
        public bool IsInsurrance { get; set; }
        public string SellTeam { get; set; }
        public string PartnerName { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
