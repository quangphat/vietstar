using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities;
using VietStar.Entities.Courier;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class CourierRepository : RepositoryBase, ICourierRepository
    {
        protected readonly ILogRepository _rpLog;
        public CourierRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }

        public async Task<CourierIndexModel> GetByIdAsync(int id)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<CourierIndexModel>("sp_GetHosoCourierById", new { id = id },
                    commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<List<CourierIndexModel>> GetsAsync(string freeText,
            DateTime fromDate
            , DateTime toDate
            , int dateType = 2
            , string status = null
            , int page = 1
            , int limit = 10
            , int assigneeId = 0
            , int groupId = 0
            , int provinceId = 0
            , string saleCode = null
            , int userId = 0)
        {
            status = string.IsNullOrWhiteSpace(status) ? string.Empty : status;
            ProcessInputPaging(ref page, ref limit, out offset);
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<CourierIndexModel>("sp_GetHosoCourier",
                    new { freeText, fromDate, toDate, dateType, status, page, limit_tmp = limit, assigneeId, groupId, provinceId, saleCode, userId },
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<BaseResponse<int>> CreateAsync(CourierSql model, int groupId = 0)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var param = GetParams(model, ignoreKey: new string[] {
                    nameof(model.CreatedTime),
                    nameof(model.UpdatedTime),
                    nameof(model.UpdatedBy),
                    nameof(model.AssigneeIds)

                }, outputParam: "Id");
                    await con.ExecuteAsync("sp_InsertHosoCourrier", param, commandType: CommandType.StoredProcedure);
                    return BaseResponse<int>.Create(param.Get<int>("Id"));
                }
            }
            catch (Exception e)
            {
                return BaseResponse<int>.Create(0, GetException(e));
            }

        }

        public async Task<BaseResponse<bool>> ImportAsync(List<CourierSql> models, int groupId = 0)
        {
            try
            {
                using (var con = GetConnection())
                {
                    foreach (var model in models)
                    {
                        var param = GetParams(model, ignoreKey: new string[] {
                        nameof(model.CreatedTime),
                        nameof(model.Id),
                        nameof(model.UpdatedTime),
                        nameof(model.UpdatedBy),
                        nameof(model.AssigneeIds)

                });
                        await con.ExecuteAsync("sp_Courier_Import", param, commandType: CommandType.StoredProcedure);

                    }
                }
                return BaseResponse<bool>.Create(true);
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }

        }

        public async Task<BaseResponse<bool>> UpdateAsync(CourierSql model)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var param = GetParams(model, ignoreKey: new string[] {
                    nameof(model.CreatedTime),
                    nameof(model.UpdatedTime),
                    nameof(model.AssigneeIds),
                    nameof(model.CreatedBy)
                });

                    await con.ExecuteAsync("sp_UpdateHosoCourier", param, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }


        }

        public async Task<bool> InsertCourierAssigneeAsync(int courierId, int assigneeId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("CourierId", courierId);
                p.Add("AssigneeId", assigneeId);
                await con.ExecuteAsync("sp_CourierAssignee_Insert", p, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
    }
}

