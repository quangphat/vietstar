using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Company;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface ICompanyRepository
    {
        Task<CompanySql> GetByIdAsync(int id);
        Task<List<CompanyIndexModel>> GetsAsync(
            string freeText,
            int page,
            int limit);
        Task<BaseResponse<int>> CreateAsync(CompanySql model, int createBy);
        Task<BaseResponse<bool>> UpdateAsync(CompanySql model, int updateBy);
    }
}

