using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.Note;
using VietStar.Repository.Interfaces;

namespace VietStar.Business
{
    public class NoteBusiness : BaseBusiness, INoteBusiness
    {
        protected readonly INoteRepository _rpNote;
        public NoteBusiness(IMapper mappr,
            INoteRepository noteRepository,
            CurrentProcess process) : base(mappr, process)
        {
            _rpNote = noteRepository;
        }

        public async Task<bool> AddNoteAsync(NoteAddRequest model)
        {
            if(model==null || string.IsNullOrWhiteSpace(model.Content) || model.ProfileId <=0 || model.ProfileTypeId <=0)
            {
                return  ToResponse(false ,Errors.invalid_data);
            }
            await _rpNote.AddNoteAsync(new NoteAddModel {
                ProfileId = model.ProfileId,
                ProfileTypeId = model.ProfileTypeId,
                Content = model.Content,
                UserId = _process.User.Id
            });
            return true;
        }

        public async Task<List<NoteViewModel>> GetByProfileIdAsync(int profileId, int profileTypeId)
        {
            var result = await _rpNote.GetNoteByTypeAsync(profileId, profileTypeId);
            return result;
        }
    }
}
