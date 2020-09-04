using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class NoteAddRequestModel : MCreditRequestModelBase
    {
        public string Id { get; set; }
        public string Content { get; set; }
    }
}
