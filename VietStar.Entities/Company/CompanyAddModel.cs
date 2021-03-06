﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Company
{
    public class CompanyAddModel
    {
        public string FullName { get; set; }
        public DateTime CheckDate { get; set; }
        public string LastNote { get; set; }
        public int PartnerId { get; set; }
        public int Status { get; set; }
        public string TaxNumber { get; set; }
        public int CatType { get; set; }
    }
}
