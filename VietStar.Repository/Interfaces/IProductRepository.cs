using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Commons;
using VietStar.Entities.Product;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<BaseResponse<bool>> DeleteByIdAsync(int id, int updateBy);
        Task<ProductIndexModel> GetByIdAync(int id);
        Task<List<OptionSimple>> GetByPartnerIdsAync(int partnerId, int orgId);
        Task<List<ProductIndexModel>> GetsAync(int orgId,int page =1, int limit =10, int partnerId =0);
        Task<BaseResponse<int>> CreateAsync(ProductCreateModel model, int createBy, int orgId);
        Task<BaseResponse<bool>> UpdateAsync(ProductUpdateModel model, int updatedBy);
        Task<int> InsertManyByParameterAsync(List<DynamicParameters> inputParams, int userId, int orgId);
    }
}

