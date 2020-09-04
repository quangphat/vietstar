using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;
using VietStar.Entities.Employee;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface IEmployeeBusiness
    {
        Task<bool> DeleteAsync(int userId);
        Task<bool> ResetPassordAsync(ChangePasswordModel model);
        Task<bool> UpdateAsync(UserEditModel model);
        Task<UserSql> GetByIdAsync(int userId);
        Task<int> CreateAsync(UserCreateModel model);
        Task<List<OptionSimple>> GetByProvinceIdAsync(int provinceId);
        Task<List<OptionSimple>> GetSalesAsync();
        Task<bool> GetStatusAsync(int userId);
        Task<Account> LoginAsync(LoginModel model);
        Task<List<OptionSimple>> GetMemberByGroupIdAsync(int groupId);
        Task<List<OptionSimple>> GetAllEmployeePagingAsync(int page, string freeText);
        Task<List<OptionSimple>> GetAllEmployeeAsync();
        Task<DataPaging<List<EmployeeViewModel>>> SearchsAsync(int role, string freeText, int page = 1, int limit = 10);
        Task<List<OptionSimple>> GetRoleListAsync();
    }
}
