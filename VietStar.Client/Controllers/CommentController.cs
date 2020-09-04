using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Note;

namespace VietStar.Client.Controllers
{
    [Authorize]
    public class CommentController : VietStarBaseController
    {
        protected readonly INoteBusiness _bizNote;
        public CommentController(CurrentProcess currentProcess, INoteBusiness noteBusiness):base(currentProcess)
        {
            _bizNote = noteBusiness;
        }
        [HttpPost("comment")]
        public async Task<IActionResult> AddComment([FromBody] NoteAddRequest model)
        {
            var result = await _bizNote.AddNoteAsync(model);
            return ToResponse(result);
        }
        [HttpGet("comment/{profileId}/{profileTypeId}")]
        public async Task<IActionResult> GetComments(int profileId, int profileTypeId)
        {
            var result = await _bizNote.GetByProfileIdAsync(profileId, profileTypeId);
            return ToResponse(result);
        }
    }
}