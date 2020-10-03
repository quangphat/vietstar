using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VietStar.Client.Models
{
    public class ExceptionDetail
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
