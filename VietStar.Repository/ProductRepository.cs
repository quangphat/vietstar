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
using VietStar.Entities.Product;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class ProductRepository : RepositoryBase, IProductRepository
    {
        protected readonly ILogRepository _rpLog;
        public ProductRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }

        public async Task<ProductIndexModel> GetByIdAync(int id)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<ProductIndexModel>("sp_Product_GetById", new { id }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<List<OptionSimple>> GetByPartnerIdsAync(int partnerId,int orgId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_SAN_PHAM_VAY_LayDSByID_v2", new { partnerId, orgId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<ProductIndexModel>> GetsAync(int orgId, int page = 1, int limit = 10, int partnerId = 0)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<ProductIndexModel>("sp_Product_Gets", new { page, limit_tmp = limit, partnerId, orgId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<BaseResponse<int>> CreateAsync(ProductCreateModel model, int createBy, int orgId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var p = AddOutputParam("id");
                    p.Add("partnerId", model.PartnerId);
                    p.Add("code", model.Code);
                    p.Add("productName", model.ProductName);
                    p.Add("orgId",orgId);
                    p.Add("createdby", createBy);
                    
                    await con.ExecuteAsync("sp_Product_Insert", p, commandType: CommandType.StoredProcedure);
                    return BaseResponse<int>.Create(p.Get<int>("id"));
                }
            }
            catch (Exception e)
            {
                return BaseResponse<int>.Create(0, GetException(e));
            }
        }

        public async Task<BaseResponse<bool>> DeleteByIdAsync(int id, int updateBy)
        {
            try
            {
                using (var con = GetConnection())
                {
                   
                    await con.ExecuteAsync("sp_Product_Delete", new { id, updateBy}, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }
        }

        public async Task<BaseResponse<bool>> UpdateAsync(ProductUpdateModel model, int updatedBy)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("id", model.Id);
                    p.Add("partnerId", model.PartnerId);
                    p.Add("code", model.Code);
                    p.Add("productName", model.ProductName);
                    p.Add("updatedBy", updatedBy);
                    await con.ExecuteAsync("sp_Product_Update", p, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }
        }

        public async Task<int> InsertManyByParameterAsync(List<DynamicParameters> inputParams, int userId, int orgId)
        {
            int successRowCount = 0;
            try
            {
                using (var con = GetConnection())
                {
                    foreach (var par in inputParams)
                    {
                        par.Add("id", 0, DbType.Int32, ParameterDirection.Output);
                        par.Add("CreatedBy", userId);
                        par.Add("OrgId", orgId);
                        await con.ExecuteAsync("sp_Product_Insert", par, commandType: CommandType.StoredProcedure);
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
    }
}

