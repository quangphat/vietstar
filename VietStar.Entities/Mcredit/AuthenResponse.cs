using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class AuthenResponse
    {
        public AuthenResponseObj Obj { get; set; }
        public List<MCreditCity> Cities { get; set; }
        public List<MCreditLoanPeriod> LoanPeriods { get; set; }
        public List<MCreditProduct> Products { get; set; }
        public List<MCreditlocations> Locations { get; set; }
        public Dictionary<string, string> ProfileStatus { get; set; }
    }
}
