using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingOffice.Infrastructures
{
    public class RoleConsts
    {
        public const string admin = "admin";
        public const string hoso_write = "hoso.write" + "," + admin;
        public const string hoso_duyet = "hoso.duyet";
        public const string hoso_read = "hoso.read" + "," + admin;
    }
}
