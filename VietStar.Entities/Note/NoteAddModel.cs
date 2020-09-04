using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Note
{
    public class NoteAddModel
    {
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public string Content { get; set; }
        public int ProfileTypeId { get; set; }
        public DateTime CommentTime { get; set; }
    }
}
