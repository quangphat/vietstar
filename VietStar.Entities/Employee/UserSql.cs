﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Employee
{
    public class UserSql : SqlBaseModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }
        public string Code { get; set; }
        public int OrgId { get; set; }
    }
}
