using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VietStar.Client.Infrastructures
{
    public static class HttpHelper
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            StringValues stringValues;
            if (request.Headers != null && request.Headers.TryGetValue("X-Requested-With", out stringValues))
                return string.Equals((string)stringValues, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
            return false;
        }
    }
}
