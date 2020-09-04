using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Employee
{
    public class ChangePasswordModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
