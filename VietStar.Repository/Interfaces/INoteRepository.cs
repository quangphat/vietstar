using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Note;

namespace VietStar.Repository.Interfaces
{
    public interface INoteRepository
    {
        Task AddNoteAsync(NoteAddModel model);
        Task<List<NoteViewModel>> GetNoteByTypeAsync(int profileId, int profileTypeId);
    }
}
