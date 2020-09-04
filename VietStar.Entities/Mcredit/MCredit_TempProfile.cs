using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class MCredit_TempProfile : SqlBaseModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Hometown { get; set; }

        public DateTime BirthDay { get; set; }

        public string Phone { get; set; }

        public string IdNumber { get; set; }

        public string CCCDNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public bool IsAddr { get; set; }

        public bool Gender { get; set; }

        public int ProvinceId { get; set; }

        public int? DistrictId { get; set; }
        public string Address { get; set; }

        public string SaleNumber { get; set; }

        public string ProductCode { get; set; }

        public string LoanPeriodCode { get; set; }
        public string LocSignCode { get; set; }

        public decimal LoanMoney { get; set; }

        public bool IsInsurrance { get; set; }

        public int Status { get; set; }
        public string isInsur
        {
            get
            {
                return IsInsurrance == true ? "true" : "false";
            }
        }
        public string isAddr
        {
            get
            {
                return IsAddr == true ? "true" : "false";
            }
        }

        public bool IsDeleted { get; set; }
        public string LastNote { get; set; }
        public string MCId { get; set; }
        public int SaleId { get; set; }
        public string SaleName { get; set; }
        public string CatNumber { get; set; }
        public string ComName { get; set; }
        public string CatName { get; set; }
    }
}
