using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class AuthenMcRequestModel
    {
        public int UserId { get; set; }
        public int[] TableToUpdateIds { get; set; }
    }
}
