using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.CheckDup;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface ICheckDupBusiness
    {
        Task<bool> UpdateAsync(CheckDupEditModel model);
        Task<List<CheckDupNoteViewModel>> GetNotesAsync(int checkDupId);
        Task<DataPaging<List<CheckDupIndexModel>>> GetsAsync(
            string freeText,
            int page,
            int limit);
        Task<CheckDupAddSql> GetByIdAsync(int id);
        Task<int> CreateAsync(CheckDupAddModel model);
    }
}
