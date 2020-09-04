using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Note
{
    public class NoteAddRequest
    {
        public int ProfileId { get; set; }
        public int ProfileTypeId { get; set; }
        public string Content { get; set; }
    }
}
