using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VietStar.Entities.Commons;

namespace VietStar.Entities.Collection
{
    public class RevokeDebtSearch : Pagination
    {

        public int Id { get; set; }

        public string AgreementNo { get; set; }

        public string CustomerName { get; set; }

        public string LastestPaymentDate { get; set; }

        public string PaymentStore { get; set; }

        public string OSPri { get; set; }

        public string TotalCurros { get; set; }

        public string LateFee { get; set; }

        public string LiquidationFee { get; set; }

        public string LateDate { get; set; }

        public string InterestrateScheme { get; set; }

        public string InstallmentPeriod { get; set; }

        public string InstallmentNo { get; set; }

        public string BillAmountOfCurrentMonth { get; set; }

        public string ProductName { get; set; }

        public string ProductBrand { get; set; }

        public string CashPrice { get; set; }

        public string DepositAmount { get; set; }

        public string FinancePrice { get; set; }

        public string FirstDueDate { get; set; }

        public string AgentCode { get; set; }

        public string Gender { get; set; }

        public string Age { get; set; }

        public string AgreementDate { get; set; }

        public string MobilePhone { get; set; }

        public string HomePhone { get; set; }

        public string CompanyPhone { get; set; }

        public string TotalPayableAmount { get; set; }

        public string LastPaymentAmount { get; set; }

        public string TotalPaidAmount { get; set; }

        public string FirstPaymentAmount { get; set; }

        public string FinalDueDate { get; set; }

        public string FinalPaymentAmount { get; set; }

        public string ReferenceName { get; set; }

        public string RefPhone { get; set; }

        public string Relative { get; set; }

        public string IdCardNumber { get; set; }

        public string Bod { get; set; }

        public string PermanentAddress { get; set; }

        public string CompanyName { get; set; }

        public string Department { get; set; }

        public string WorkAddress { get; set; }

        public DateTime CreatedTime { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }

        public int UpdatedBy { get; set; }

        public string AssigneeGroupIds { get; set; }

        public string AssigneeIds { get; set; }

        public bool IsDeleted { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string PartnerName { get; set; }
        public string LastNote { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int GroupId { get; set; }
        public string AssigneeId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AssigneeIds))
                    return "0";
                return AssigneeIds.Split('.').FirstOrDefault();
            }
        }
        public string AssigneeName { get; set; }

        public string AssigneeCode { get; set; }
    }
}
