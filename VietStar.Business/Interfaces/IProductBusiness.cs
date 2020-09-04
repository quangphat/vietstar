using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Product;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface IProductBusiness
    {
        Task<bool> DeleteByIdAync(int id);
        Task<ProductIndexModel> GetByIdAync(int id);
        Task<DataPaging<List<ProductIndexModel>>> GetsAsync(
            int page = 1
           , int limit = 10
           , int partnerId = 0
           );
        Task<int> CreateAsync(ProductCreateModel model);
        Task<bool> UpdateAsync(ProductUpdateModel model);
        Task<string> InsertFromFileAsync(IFormFile file);
    }
}
