using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.ViewModels
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Rememberme { get; set; }
    }
}
