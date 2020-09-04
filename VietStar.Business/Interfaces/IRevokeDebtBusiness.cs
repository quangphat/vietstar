using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Collection;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface IRevokeDebtBusiness
    {
        Task<string> InsertFromFileAsync(IFormFile file);
        Task<DataPaging<List<RevokeDebtSearch>>> SearchAsync(DateTime? fromDate = null,
            DateTime? toDate = null,
            int dateType = 1,
            int groupId = 0,
            int assigneeId = 0,
            string status = null,
            int processStatus = -1,
            string freeText = null,
            int page = 1,
            int limit = 10);
        Task<RevokeDebtSearch> GetByIdAsync(int profileId);
        Task<bool> DeleteByIdAsync( int profileId);
        Task<bool> UpdateStatusAsync(int profileId, int status);
        Task<bool> UpdateSimpleAsync(RevokeSimpleUpdate model, int profileId);
        Task<string> ExportAsync(string contentRootPath, DateTime? fromDate = null,
            DateTime? toDate = null,
            int dateType = 1,
            int groupId = 0,
            int assigneeId = 0,
            string status = null,
            int processStatus = -1,
            string freeText = null,
            int page = 1,
            int limit = 10);
    }
}
