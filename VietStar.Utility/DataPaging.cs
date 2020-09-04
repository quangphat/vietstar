using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace VietStar.Utility
{
    public class DataPaging
    {
        public static DataPaging<T> Create<T>(T data, long totalRecord) where T : class
        {
            DataPaging<T> d = new DataPaging<T>
            {
                Datas = data,
                TotalRecord = (data == null) ? 0 : totalRecord
            };
            return d;
        }

        [DataMember]
        public long TotalRecord { get; set; }
    }
    public class DataPaging<T> : DataPaging where T : class
    {
        [DataMember]
        public T Datas { get; set; }
    }
}
