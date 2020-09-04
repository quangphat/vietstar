using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Product
{
    public class ProductUpdateModel
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
    }
}
