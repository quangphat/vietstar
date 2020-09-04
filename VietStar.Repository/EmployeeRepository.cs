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
using VietStar.Entities.Employee;
using VietStar.Entities.GroupModels;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;

namespace VietStar.Repository
{
    public class EmployeeRepository : RepositoryBase, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<BaseResponse<bool>> DeleteAsync(int userId, int deleteId)
        {
            var p = new DynamicParameters();
            p.Add("userId", userId);

            p.Add("deleteId", deleteId);
            try
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_Employee_Delete_v3", p, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }

        }

        public async Task<BaseResponse<bool>> ResetPassordAsync(int id, string password, int updatedBy)
        {

            try
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_ResetPassword",
                        new
                        {
                            id,
                            password,
                            updatedBy
                        }, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }

        }

        public async Task<BaseResponse<bool>> UpdateAsync(UserSql model)
        {
            var p = new DynamicParameters();
            p.Add("id", model.Id);
            p.Add("fullName", model.FullName);
            p.Add("phone", model.Phone);
            p.Add("roleId", model.RoleId);
            p.Add("email", model.Email);
            p.Add("UpdatedBy", model.UpdatedBy);
            try
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_Employee_UpdateUser_v2", p, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true); ;
                }
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.Create(false, GetException(e));
            }


        }

        public async Task<UserSql> GetByIdAsync(int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<UserSql>("sp_GetEmployeeById_v2", new { userId }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }

        public async Task<UserSql> GetByUserNameAsync(string userName, int userId)
        {

            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<UserSql>("sp_Employee_GetByUsername", new { userName, userId }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }

        public async Task<UserSql> GetByCodeAsync(string code, int userId)
        {

            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<UserSql>("sp_Employee_GetByCode", new { code, userId }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }



        public async Task<BaseResponse<int>> CreateAsync(UserSql model)
        {
            var p = AddOutputParam("id");
            p.Add("code", model.Code);
            p.Add("userName", model.UserName);
            p.Add("password", model.Password);
            p.Add("fullName", model.FullName);
            p.Add("phone", model.Phone);
            p.Add("roleId", model.RoleId);
            p.Add("email", model.Email);
            p.Add("createdby", model.CreatedBy);
            try
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_Employee_InsertUser_v2", p, commandType: CommandType.StoredProcedure);
                    return BaseResponse<int>.Create(p.Get<int>("id"));
                }
            }
            catch (Exception e)
            {
                return BaseResponse<int>.Create(0, GetException(e));
            }


        }

        public async Task<List<int>> GetPeopleCanViewMyProfileAsync(int profileId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<int>("sp_MCProfilePeople_GetPeopleCanViewProfile", new { profileId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<OptionSimple>> GetRoleListAsync(int userId)
        {

            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_Role_GetRoles", new { userId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<OptionSimple> GetEmployeeByCodeAsync(string code, int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<OptionSimple>("sp_Employee_GetByCode", new { code, userId }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<List<OptionSimple>> GetByProvinceIdAsync(int provinceId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("spGetAllUserByProvinceId", new { provinceId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }


        }

        public async Task<bool> GetStatusAsync(int userId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<bool>("sp_Employee_GetStatus", new
                {
                    userId
                }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<List<string>> GetPermissionsAsync(string roleCode)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<string>("sp_getPermissionByRoleCode", new
                {
                    roleCode,

                }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<Account> LoginAsync(string userName, string password)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<Account>("sp_Employee_Login", new
                {
                    userName,
                    password
                }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }

        public async Task<List<OptionSimple>> GetMemberByGroupIdIncludeChildAsync(int groupId, int userId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<OptionSimple>("sp_Employee_Group_LayDSChonThanhVienNhomCaCon_v3",
                        new { groupId, userId },
                        commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<OptionSimple>> GetCouriersAsync(int orgId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<OptionSimple>("sp_NHAN_VIEN_LayDSCourierCode_v2",
                        new { orgId },
                        commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<OptionSimple>> GetMemberByGroupIdAsync(int groupId, int userId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<OptionSimple>("sp_Employee_Group_LayDSChonThanhVienNhom_v3",
                        new { groupId, userId },
                        commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<OptionSimple>> GetAllEmployeeAsync(int orgId)
        {
            using (var con = GetConnection())
            {
                var rs = await con.QueryAsync<OptionSimple>("sp_Employee_GetFull", new { orgId }, commandType: CommandType.StoredProcedure);
                return rs.ToList();
            }
        }

        public async Task<List<OptionSimple>> GetAllEmployeePagingAsync(int orgId, int page, string freeText)
        {
            using (var con = GetConnection())
            {
                var rs = await con.QueryAsync<OptionSimple>("sp_Employee_GetPaging", new { orgId, page, freeText }, commandType: CommandType.StoredProcedure);
                return rs.ToList();
            }
        }

        public async Task<List<EmployeeViewModel>> GetsAsync(
           int roleId,
           string freeText,
           int page,
           int limit, int OrgId)
        {
            ProcessInputPaging(ref page, ref limit, out int offset);
            var p = new DynamicParameters();
            p.Add("freeText", freeText);
            p.Add("page", page);
            p.Add("roleId", roleId);
            p.Add("limit", limit);
            p.Add("OrgId", OrgId);

            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<EmployeeViewModel>("sp_GetEmployees_v2", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }

        }

    }
}
