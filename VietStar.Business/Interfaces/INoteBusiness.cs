using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Note;

namespace VietStar.Business.Interfaces
{
    public interface INoteBusiness
    {
        Task<bool> AddNoteAsync(NoteAddRequest model);
        Task<List<NoteViewModel>> GetByProfileIdAsync(int profileId, int profileTypeId);
    }
}
