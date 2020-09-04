using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class NoteResponseModel : MCResponseModelBase
    {
        public int Total { get; set; }
        public List<NoteObj> objs { get; set; }

    }
    public class NoteObj
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreateUserName { get; set; }
        public string CreateUserAvatar { get; set; }
        public string CreateDated { get; set; }
    }
}
