using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Courier;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface ICourierBusiness
    {
        Task<bool> InsertFromFileAsync(IFormFile file);
        Task<DataPaging<List<CourierIndexModel>>> GetsAsync(string freeText,
            DateTime? fromDate
            , DateTime? toDate
            ,int dateType = 2
            , string status = null
            , int page = 1
            , int limit = 10
            , int assigneeId = 0
            , int groupId = 0
            , int provinceId = 0
            , string saleCode = null
            );
        Task<string> ExportAsync(string contentRootPath,
            string freeText,
            DateTime? fromDate
            , DateTime? toDate
            , int dateType = 2
            , string status = null
            , int page = 1
            , int limit = 10
            , int assigneeId = 0
            , int groupId = 0
            , int provinceId = 0
            , string saleCode = null);
        Task<int> CreateAsync(CourierAddModel model);
        Task<bool> UpdateAsync(CourierUpdateModel model);
        Task<CourierIndexModel> GetByIdAsync(int id);
    }
}
