using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Company;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface ICompanyBusiness
    {
        Task<bool> UpdateAsync(CompanyEditModel model);
        Task<CompanySql> GetByIdAsync(int id);
        Task<DataPaging<List<CompanyIndexModel>>> SearchsAsync(string freeText, int page, int limit);
        Task<int> CreateAsync(CompanyAddModel model);
    }
}
