using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.Collection;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class RevokeDebtRepository : RepositoryBase, IRevokeDebtRepository
    {
        protected readonly ILogRepository _rpLog;
        public RevokeDebtRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }

        public async Task<List<RevokeDebtSearch>> SearchAsync(int userId,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int dateType = 1,
            int groupId = 0,
            int assigneeId = 0,
            string status = null,
            int processStatus = -1,
            string freeText = null,
            int page = 1,
            int limit = 10)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<RevokeDebtSearch>("sp_RevokeDebt_Search",
                    new { freeText, page, limit_tmp = limit, status, groupId, userId, assigneeId, fromDate, toDate, dateType, processStatus }
                    , commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<int> InsertManyByParameterAsync(List<DynamicParameters> inputParams, int userId)
        {
            int successRowCount = 0;
            try
            {
                using (var con = GetConnection())
                {
                    foreach(var par in inputParams)
                    {
                        par.Add("CreatedBy", userId);
                        var assigneeIds = par.Get<string>("AssigneeIds");
                        int assigneeId = 0;
                        assigneeId = !string.IsNullOrWhiteSpace(assigneeIds) ? Convert.ToInt32(assigneeIds.Split('.').FirstOrDefault()) : 0;
                        par.Add("AssigneeId", assigneeId);
                        await con.ExecuteAsync("sp_insert_RevokeDebt",
                        par, commandType: CommandType.StoredProcedure);
                        successRowCount++;
                    }
                }
                return successRowCount;

            }
            catch (Exception e)
            {
                return successRowCount;
            }
            
        }

        public async Task<RevokeDebtSearch> GetByIdAsync(int profileId, int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<RevokeDebtSearch>("sp_RevokeDebt_GetById", new { profileId, userId }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<bool> DeleteByIdAsync(int userId, int profileId)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_RevokeDebt_Delete",
                     new { profileId, userId }
                     , commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> UpdateStatusAsync(int userId, int profileId, int status)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_RevokeDebt_UpdateStatus",
                     new { profileId, userId, status }
                     , commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<bool> UpdateSimpleAsync(RevokeSimpleUpdate model, int updateBy, int profileId)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_RevokeDebt_UpdateSimple",
                     new { profileId, updateBy, model.Status, model.AssigneeId, model.DistrictId, model.ProvinceId, model.GroupId }
                     , commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}

