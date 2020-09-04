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
using VietStar.Entities.Company;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class CompanyRepository : RepositoryBase, ICompanyRepository
    {
        protected readonly ILogRepository _rpLog;
        public CompanyRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }

        public async Task<CompanySql> GetByIdAsync(int id)
        {
            string sql = $"select * from Company where Id = @id";
            using (var con = GetConnection())
            {
                var customer = await con.QueryFirstOrDefaultAsync<CompanySql>(sql, new
                {
                    id
                }, commandType: CommandType.Text);
                return customer;
            }

        }

        public async Task<BaseResponse<bool>> UpdateAsync(CompanySql model, int updateBy)
        {
            var p = new DynamicParameters();
            p.Add("id", model.Id);
            p.Add("fullname", model.FullName);
            p.Add("checkdate", model.CheckDate);
            p.Add("status", model.Status);
            p.Add("note", string.IsNullOrWhiteSpace(model.LastNote) ? null : model.LastNote);
            p.Add("TaxNumber", model.TaxNumber);
            p.Add("CatType", model.CatType);
            p.Add("updatedtime", DateTime.Now);
            p.Add("updatedby", updateBy);
            try
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_UpdateCompany", p, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }
            }
            catch(Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }

        }

        public async Task<BaseResponse<int>> CreateAsync(CompanySql model, int createBy)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var p = AddOutputParam("id");
                    p.Add("fullname", model.FullName);
                    p.Add("checkdate", model.CheckDate);
                    p.Add("note", model.LastNote);
                    p.Add("createdtime", DateTime.Now);
                    p.Add("createdby", createBy);
                    p.Add("TaxNumber", model.TaxNumber);
                    p.Add("PartnerId", model.PartnerId);
                    p.Add("CatType", model.CatType);
                    await con.ExecuteAsync("sp_InsertCompany", p, commandType: CommandType.StoredProcedure);
                    return BaseResponse<int>.Create(p.Get<int>("id"));
                }
            }
            catch(Exception e)
            {
                return BaseResponse<int>.Create(0, GetException(e));
            }
        }

        public async Task<List<CompanyIndexModel>> GetsAsync(
            string freeText,
            int page,
            int limit)
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
            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<CompanyIndexModel>("sp_GetCompany_v2", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }
    }
}

