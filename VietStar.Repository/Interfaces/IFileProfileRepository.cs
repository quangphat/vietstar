using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.FileProfile;
using VietStar.Entities.Mcredit;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IFileProfileRepository
    {
        Task<List<FileUploadModel>> GetFilesByProfileIdAsync(int profileType, int profileId);
        Task<bool> DeleteByIdAsync(int fileId, string guidId);
        Task<BaseResponse<int>> Add(ProfileFileAddSql model);
        Task<BaseResponse<int>> AddMCredit(MCProfileFileSqlModel model);
        Task<List<FileProfileType>> GetByType(int profileType);

        Task<bool> UpdateFileMCProfileByIdAsync(int profileId, string mcId);
    }
}

