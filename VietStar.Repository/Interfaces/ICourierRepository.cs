using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Courier;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface ICourierRepository
    {
        Task<List<CourierIndexModel>> GetsAsync(
           string freeText,
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
            , int userId = 0);
        Task<BaseResponse<bool>> UpdateAsync(CourierSql model);
        Task<CourierIndexModel> GetByIdAsync(int id);
        Task<BaseResponse<int>> CreateAsync(CourierSql model, int groupId = 0);
        Task<bool> InsertCourierAssigneeAsync(int courierId, int assigneeId);
        Task<BaseResponse<bool>> ImportAsync(List<CourierSql> models, int groupId = 0);
    }
}

