using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class MCreditProduct
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string MaxLoanAmount { get; set; }

        public string MinLoanAmount { get; set; }

        public string MaxTenor { get; set; }

        public string MinTenor { get; set; }

        public string IsCheckCat { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string McId { get; set; }

    }
}
