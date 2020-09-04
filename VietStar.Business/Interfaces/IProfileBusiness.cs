using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Profile;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface IProfileBusiness
    {
        Task<bool> UpdateProfile(ProfileAdd model);
        Task<DataPaging<List<ProfileIndexModel>>> GetsAsync(DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1
            , int groupId = 0
            , int memberId = 0
            , string status = null
            , string freeText = null
            , string sort = "desc"
            , string sortField = "updatedtime"
            , int page = 1, int limit = 20);
        Task<string> ExportAsync(string contentRootPath, DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1
            , int groupId = 0
            , int memberId = 0
            , string status = null
            , string freeText = null
            , string sort = "desc"
            , string sortField = "updatedtime"
            , int page = 1
            , int limit = 20);
        Task<int> CreateAsync(ProfileAdd model);
        Task<ProfileEditView> GetByIdAsync(int profileId);
    }
}
