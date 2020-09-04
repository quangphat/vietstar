using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Commons;
using VietStar.Entities.Employee;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<BaseResponse<bool>> ResetPassordAsync(int id, string password, int updatedBy);
        Task<BaseResponse<bool>> UpdateAsync(UserSql model);
        Task<UserSql> GetByIdAsync(int userId);
        Task<UserSql> GetByUserNameAsync(string userName, int userId);
        Task<BaseResponse<int>> CreateAsync(UserSql model);
        Task<OptionSimple> GetEmployeeByCodeAsync(string code, int userId);
        Task<List<OptionSimple>> GetByProvinceIdAsync(int provinceId);
        Task<List<OptionSimple>> GetCouriersAsync(int orgId);
        Task<bool> GetStatusAsync(int userId);
        Task<Account> LoginAsync(string userName, string password);
        Task<List<string>> GetPermissionsAsync(string  roleCode);
        Task<List<OptionSimple>> GetMemberByGroupIdIncludeChildAsync(int groupId, int userId);
        Task<List<OptionSimple>> GetMemberByGroupIdAsync(int groupId, int userId);
        Task<List<int>> GetPeopleCanViewMyProfileAsync(int profileId);
        Task<List<OptionSimple>> GetAllEmployeeAsync(int orgId);
        Task<List<OptionSimple>> GetAllEmployeePagingAsync(int orgId, int page, string freeText);
        Task<List<EmployeeViewModel>> GetsAsync(
           int roleId,
           string freeText,
           int page,
           int limit, int OrgId);
        Task<List<OptionSimple>> GetRoleListAsync(int userId);
        Task<UserSql> GetByCodeAsync(string code, int userId);
        Task<BaseResponse<bool>> DeleteAsync(int userId, int deleteId);
    }
}
