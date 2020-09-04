using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Profile
{
    public class ProfileEditView
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string SalePhone { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string Cmnd { get; set; }
        public int Gender { get; set; }
        public int DistrictId { get; set; }
        public int ProvinceId { get; set; }
        public string Address { get; set; }
        public int CourierId { get; set; }
        public bool IsInsurrance { get; set; }
        public int Period { get; set; }
        public decimal LoanAmount { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime? CmndDay { get; set; }
        public int? PartnerId { get; set; }
        public int ProductId { get; set; }
        public int SaleId { get; set; }
    }
}
