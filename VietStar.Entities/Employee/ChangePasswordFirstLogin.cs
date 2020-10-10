using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Employee
{
    public class ChangePasswordFirstLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordNew { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
