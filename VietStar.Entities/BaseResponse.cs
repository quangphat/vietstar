using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities
{
    public class BaseResponse<T>
    {
        public static BaseResponse<T> Create(T dat, string err  =null)
        {
            return new BaseResponse<T>
            {
                data = dat,
                error = err,
                success = string.IsNullOrWhiteSpace(err)
            };
        }
        public string error { get; set; }
        public T data { get; set; }
        public bool success { get; set; }
    }
}
