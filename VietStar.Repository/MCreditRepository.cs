using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities;
using VietStar.Entities.Commons;
using VietStar.Entities.Mcredit;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class MCreditRepository : RepositoryBase, IMCreditRepository
    {
        protected readonly ILogRepository _rpLog;
        public MCreditRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }

        public async Task<int> GetProfileIdByIdNumberAsync(string idNumber)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<int>("sp_MCProfile_CheckIsExist", new { IDNumber = idNumber }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<bool> UpdateSaleAsyncAsync(UpdateSaleModel model, int profileId)
        {

            using (var con = GetConnection())
            {

                await con.ExecuteAsync("sp_MCredit_TempProfile_update_Sale", new
                {
                    profileId,
                    model.SaleId,
                    model.SaleNumber,
                    model.SaleName
                }, commandType: CommandType.StoredProcedure);
            }
            return true;
        }
        public async Task<bool> DeleteMCTableDatasAsync(int type)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_deleteMCTable", new { type }, commandType: CommandType.StoredProcedure);
                return true;
            }

        }

        public async Task<BaseResponse<bool>> UpdateTempProfileStatusAsync(int profileId, int status)
        {
            try
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_MCredit_UpdateStatusFromMC", new { profileId, status }, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }

        }
        public async Task<BaseResponse<MCredit_TempProfile>> GetTemProfileByIdAsync(int id)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryFirstOrDefaultAsync<MCredit_TempProfile>("sp_MCredit_TempProfile_GetById", new { id }, commandType: CommandType.StoredProcedure);
                    return BaseResponse<MCredit_TempProfile>.Create(result);
                }
            }
            catch (Exception e)
            {
                return BaseResponse<MCredit_TempProfile>.Create(null, GetException(e));
            }

        }
        public async Task<BaseResponse<MCredit_TempProfile>> GetTemProfileByMcIdAsync(string id)
        {

            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryFirstOrDefaultAsync<MCredit_TempProfile>("sp_MCredit_TempProfile_GetByMCId", new { id }, commandType: CommandType.StoredProcedure);
                    return BaseResponse<MCredit_TempProfile>.Create(result);
                }
            }
            catch (Exception e)
            {
                return BaseResponse<MCredit_TempProfile>.Create(null, GetException(e));
            }
        }
        public async Task<MCreditUserToken> GetUserTokenByIdAsyncAsync(int userId)
        {
            string sql = $"sp_MCreditUserToken_GetTokenByUserId";
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<MCreditUserToken>(sql, new
                {
                    userId
                }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }

        public async Task<bool> InsertLocationsAsync(List<MCreditlocations> locations)
        {
            List<Task> tasks = new List<Task>();
            foreach (var p in locations)
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_insert_MCreditlocations", new
                    {
                        McId = p.Id,
                        p.Code,
                        p.Addr,
                        p.Name

                    }, commandType: CommandType.StoredProcedure);
                }
            }
            //await Task.WhenAll(tasks);
            return true;
        }
        public async Task<bool> InsertProfileStatusAsync(List<OptionSimple> status)
        {
            foreach (var p in status)
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_MCreditProfileStatus_Insert", new
                    {
                        p.Code,
                        p.Name

                    }, commandType: CommandType.StoredProcedure);
                }
            }
            return true;
        }
        public async Task<bool> InsertCitiesAsync(List<MCreditCity> cities)
        {
            List<Task> tasks = new List<Task>();
            foreach (var p in cities)
            {
                using (var con = GetConnection())
                {

                    await con.ExecuteAsync("sp_insert_MCreditCity", new
                    {
                        McId = p.Id,
                        p.Code,
                        p.Name
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            // await Task.WhenAll(tasks);
            return true;
        }
        public async Task<bool> InsertLoanPeriodsAsync(List<MCreditLoanPeriod> loanPeriods)
        {
            List<Task> tasks = new List<Task>();
            foreach (var p in loanPeriods)
            {
                using (var con = GetConnection())
                {

                    await con.ExecuteAsync("sp_insert_MCreditLoanPeriod", new
                    {
                        McId = p.Id,
                        p.Code,
                        p.Name
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            //await Task.WhenAll(tasks);
            return true;
        }
        public async Task<bool> InsertProductsAsync(List<MCreditProduct> products)
        {
            List<Task> tasks = new List<Task>();
            foreach (var p in products)
            {
                using (var con = GetConnection())
                {

                    await con.ExecuteAsync("sp_insert_MCreditProduct", new
                    {
                        McId = p.Id,
                        p.Code,
                        p.Name,
                        p.IsCheckCat,
                        p.MaxLoanAmount,
                        p.MinLoanAmount,
                        p.MinTenor,
                        p.MaxTenor
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            // await Task.WhenAll(tasks);
            return true;
        }

        public async Task<bool> InsertUserTokenAsync(MCreditUserToken model)
        {
            using (var con = GetConnection())
            {

                await con.ExecuteAsync("sp_MCUserToken_Insert", new
                {
                    model.UserId,
                    model.Token
                }, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<bool> InsertPeopleWhoCanViewProfileAsync(int profileId, string peopleIds)
        {
            using (var con = GetConnection())
            {

                await con.ExecuteAsync("sp_MCProfilePeople_Insert", new
                {
                    profileId,
                    peopleIds
                }, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<List<OptionSimple>> GetMCProfileStatusListAsync()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_MCStatus_GetSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetMCLocationSimpleListAsync()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_MCLocation_GetSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetMCProductSimpleListAsync()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_MCProduct_GetSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetMCLoanPerodSimpleListAsync()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_MCLoanPeriod_GetSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetMCCitiesSimpleListAsync()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_MCCities_GetSimpleList", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<BaseResponse<int>> CreateDraftProfileAsync(MCredit_TempProfile model)
        {
            try
            {
                model.Id = 0;
                var param = GetParams(model, ignoreKey: new string[]
                {
                nameof(model.CreatedTime),
                nameof(model.UpdatedTime),
                nameof(model.isAddr),
                nameof(model.isInsur),
                nameof(model.MCId)
                }, outputParam: "Id");

                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_insert_MCredit_TempProfile", param, commandType: CommandType.StoredProcedure);
                    return BaseResponse<int>.Create(param.Get<int>("Id"));
                }
            }
            catch (Exception e)
            {
                return BaseResponse<int>.Create(0, GetException(e));
            }
        }
        public async Task<bool> DeleteByIdAsync(int profileId)
        {

            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_MCProfile_DeleteProfile", new { profileId }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<BaseResponse<bool>> UpdateMCIdAsync(int id, string mcId, int updatedBy)
        {
            try
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_MCProfile_UpdateMcId", new
                    {
                        id,
                        mcId,
                        updatedBy
                    }, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }
        }

        public async Task<BaseResponse<bool>> UpdateDraftProfileAsync(MCredit_TempProfile model)
        {
            var param = GetParams(model, ignoreKey: new string[]
            {
                nameof(model.CreatedTime),
                nameof(model.UpdatedTime),
                nameof(model.CreatedBy),
                nameof(model.isAddr),
                nameof(model.isInsur)
            });

            try
            {

                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_update_MCredit_TempProfile", param, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }
        }
        public async Task<List<TempProfileIndexModel>> GetTempProfilesAsync(int userId,
            DateTime? fromDate,
            DateTime? toDate,
            int dateType = 1,
            int page = 1,
            int limit = 10,
            string freeText = null,
            string status = null)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<TempProfileIndexModel>("sp_MCredit_TempProfile_Gets", new
                {
                    fromDate,
                    toDate,
                    dateType,
                    freeText,
                    userId,
                    page,
                    limit_tmp = limit,
                    status
                }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<int> CountTempProfilesAsync(string freeText, int userId, string status = null)
        {
            using (var con = GetConnection())
            {
                var result = await con.ExecuteScalarAsync<int>("sp_MCredit_TempProfile_Counts", new
                {
                    freeText,
                    userId,
                    status
                }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<bool> IsCheckCatAsync(string productCode)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<int>($"select   dbo.fn_MCProduct_ISCheckCat('{productCode.Trim()}')"
                , commandType: CommandType.Text);
                return result == 1 ? true : false;
            }
        }

        
    }
}

