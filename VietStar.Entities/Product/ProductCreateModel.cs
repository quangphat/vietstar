using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Product
{
    public class ProductCreateModel
    {
        public int PartnerId { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
    }
}
