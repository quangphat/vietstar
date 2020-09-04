using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Collection;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IRevokeDebtRepository
    {
        Task<int> InsertManyByParameterAsync(List<DynamicParameters> inputParams, int userId);
        Task<List<RevokeDebtSearch>> SearchAsync(int userId,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int dateType = 1,
            int groupId = 0,
            int assigneeId = 0,
            string status = null,
            int processStatus = -1,
            string freeText = null,
            int page = 1,
            int limit = 10
            );

        Task<RevokeDebtSearch> GetByIdAsync(int profileId, int userId);
        Task<bool> DeleteByIdAsync(int userId, int profileId);
        Task<bool> UpdateStatusAsync(int userId, int profileId, int status);
        Task<bool> UpdateSimpleAsync(RevokeSimpleUpdate model, int updateBy, int profileId);
    }
}

