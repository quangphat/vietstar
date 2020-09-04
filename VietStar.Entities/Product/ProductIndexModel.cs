using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities.Commons;

namespace VietStar.Entities.Product
{
    public class ProductIndexModel:Pagination
    {
        public int Id { get; set; }
        public string PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int CreatedBy { get; set; }

    }
}
