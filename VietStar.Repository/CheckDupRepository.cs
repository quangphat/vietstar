using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities;
using VietStar.Entities.CheckDup;
using VietStar.Entities.Commons;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class CheckDupRepository : RepositoryBase, ICheckDupRepository
    {
        protected readonly ILogRepository _rpLog;
        public CheckDupRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }
        public async Task<BaseResponse<bool>> UpdateAsync(CheckDupAddSql model, int updateBy)
        {
            var pars = GetParams(model, new string[]
                        {
                            nameof(model.CreatedBy),
                            nameof(model.CreatedTime),
                            nameof(model.UpdatedTime),
                            nameof(model.UpdatedBy),
                        });
            pars.Add(nameof(model.UpdatedBy), updateBy);
            try
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_UpdateCustomer_v2", pars, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }


        }
        public async Task<List<CheckDupNoteViewModel>> GetNoteByIdAsync(int customerId)
        {
            var p = new DynamicParameters();
            p.Add("customerId", customerId);
            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<CheckDupNoteViewModel>("sp_GetNotesByCustomerId", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }
        public async Task<List<CheckDupIndexModel>> GetsAsync(
            string freeText,
            int page,
            int limit,
            int userId)
        {
            page = page <= 0 ? 1 : page;
            limit = (limit <= 0 || limit >= Constants.Limit_Max_Page) ? Constants.Limit_Max_Page : limit;
            int offset = (page - 1) * limit;
            if (string.IsNullOrWhiteSpace(freeText))
                freeText = "";
            var p = new DynamicParameters();
            p.Add("freeText", freeText);
            p.Add("offset", offset);
            p.Add("limit", limit);
            p.Add("userId", userId);

            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<CheckDupIndexModel>("sp_GetCustomer_v2", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }
        public async Task<CheckDupAddSql> GetByIdAsync(int id)
        {
            string sql = $"select * from Customer where Id = @id";
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<CheckDupAddSql>(sql, new
                {
                    id
                }, commandType: CommandType.Text);
                return result;
            }
        }
        public async Task<BaseResponse<int>> CreateAsync(CheckDupAddSql model, int createdBy)
        {
            //var par = GetParams(model, new string[] {
            //    nameof(model.UpdatedBy),
            //    nameof(model.UpdatedTime),
            //    nameof(model.CreatedTime),
            //    nameof(model.CreatedBy)

            //}, "Id");
            //par.Add("CreatedBy", createdBy);
            try
            {
                using (var con = GetConnection())
                {
                    var pars = GetParams(model, new string[] {
                        nameof(model.CreatedTime),
                        nameof(model.UpdatedTime),
                        nameof(model.UpdatedBy),
                        nameof(model.CreatedBy)
                    }, nameof(model.Id));
                    pars.Add("CreatedBy", createdBy);

                    await con.ExecuteAsync("sp_InsertCustomer_v2", pars, commandType: CommandType.StoredProcedure);
                    return BaseResponse<int>.Create(pars.Get<int>(nameof(model.Id)));

                }
            }
            catch (Exception e)
            {
                return BaseResponse<int>.Create(0, GetException(e));
            }
        }
        public async Task<bool> AddNoteAsync(CheckDupNote note)
        {
            string sql = $"insert into CustomerNote(CustomerId, Note,CreatedTime,CreatedBy) values (@customerId,@note,@createdTime,@createdBy)";
            using (var con = GetConnection())
            {
                await con.ExecuteAsync(sql, new
                {
                    customerId = note.CustomerId,
                    note = note.Note,
                    createdTime = DateTime.Now,
                    createdBy = note.CreatedBy
                }, commandType: CommandType.Text);
                return true;
            }

        }
    }
}

