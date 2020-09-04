
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using VietStar.Entities.ViewModels;
using MsgPack.Serialization;

namespace KingOffice.Infrastructures
{
    public static class Utils
    {
        public static string ToJson(object value)
        {
            JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new DefaultContractResolver()
            };
            return JsonConvert.SerializeObject(value, _jsonSerializerSettings);
        }
        public static byte[] ToBinary(Account account)
        {
            using (var ms = new MemoryStream())
            {
                var packer = SerializationContext.Default.GetSerializer<Account>();
                packer.Pack(ms, account);

                return ms.ToArray();
            }
        }
        public static Account FromBinary(byte[] raw)
        {
            if (raw == null || (!raw.Any()))
                return null;
            using (var ms = new MemoryStream(raw))
            {
                var packer = SerializationContext.Default.GetSerializer<Account>();
                var obj = packer.Unpack(ms);

                return obj;
            }
        }
        public static string AccountToJson(this Account account)
        {
            return PrepareAccountJson(account);
        }

        private static string PrepareAccountJson(Account account)
        {
            if (account == null) return null;
            return JsonConvert.SerializeObject(account, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
        public static string ConvertToJson(object obj)
        {
            if (obj == null)
                return null;
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
