using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.ViewModels
{
    public class Account
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Code { get; set; }
        public string RoleCode { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Permissions { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public int OrgId { get; set; }
        public bool FirstLogin { get; set; }

        public bool isHead
        {
            get
            {
                return (RoleCode == "head") ? true : false;
            }
        }
        public bool isDev
        {
            get
            {
                return (RoleCode == "dev") ? true : false;
            }
        }
        public bool isAdmin
        {
            get
            {
                return (RoleCode == "admin" || RoleCode == "head") ? true : false;
            }
        }
        public bool isSale
        {
            get
            {
                return (RoleCode == "sale" ) ? true : false;
            }
        }
        public bool isRsmAsmSS
        {
            get
            {
                return (RoleCode == "rsm" || RoleCode == "asm" || RoleCode == "ss") ? true : false;
            }
        }
    }
}
