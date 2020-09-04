using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Infrastructures;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.Employee;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

namespace VietStar.Business
{
    public class EmployeeBusiness : BaseBusiness, IEmployeeBusiness
    {
        protected readonly IEmployeeRepository _rpEmployee;
        protected readonly IGroupRepository _rpGroup;
        public EmployeeBusiness(IEmployeeRepository employeeRepository, IGroupRepository groupRepository, IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpEmployee = employeeRepository;
            _rpGroup = groupRepository;
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            var user = await _rpEmployee.GetByIdAsync(userId);

            if (user == null)
            {
                return ToResponse(false, "Tài khoản không tồn tại");
            }
            var result = await _rpEmployee.DeleteAsync(_process.User.Id, userId);
            return ToResponse(result);
        }

        public async Task<bool> ResetPassordAsync(ChangePasswordModel model)
        {
            if (model == null || model.Id<=0)
            {
                return ToResponse(false, Errors.invalid_data);
            }

            var user = await _rpEmployee.GetByIdAsync(model.Id);

            if (user == null)
            {
                return ToResponse(false, "Tài khoản không tồn tại");
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return ToResponse(false, "Mật khẩu không được để trống");
            }
            if (model.Password.Trim().Length < 5)
            {
                return ToResponse(false, "Mật khẩu phải có ít nhất 5 ký tự");
            }
            if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                return ToResponse(false, "Mật khẩu xác thực không được để trống");
            }
            if (model.Password != model.ConfirmPassword)
            {
                return ToResponse(false, "Mật khẩu không khớp");
            }

            var result = await _rpEmployee.ResetPassordAsync(model.Id, Utils.getMD5(model.Password), _process.User.Id);
            return ToResponse(result);
        }

        public async Task<bool> UpdateAsync(UserEditModel model)
        {
            if (model == null)
            {
                return ToResponse(false, Errors.invalid_data);
            }

            if (!string.IsNullOrWhiteSpace(model.Email) && !BusinessExtensions.IsValidEmail(model.Email.Trim(), 50))
            {
                return ToResponse(false, "Email không hợp lệ");
            }

            if (model.RoleId <= 0)
            {
                return ToResponse(false, "Vui lòng chọn vai trò");
            }

            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                return ToResponse(false, Errors.invalid_username_or_pass);
            }
            var user = await _rpEmployee.GetByUserNameAsync(model.UserName.Trim(), _process.User.Id);

            if (user == null)
            {
                return ToResponse(false, "Tài khoản không tồn tại");
            }
            user = _mapper.Map<UserSql>(model);
            user.UpdatedBy = _process.User.Id;
            user.Email = string.IsNullOrWhiteSpace(user.Email) ? string.Empty : user.Email.Trim().ToLower();
            return ToResponse(await _rpEmployee.UpdateAsync(user));
        }

        public async Task<UserSql> GetByIdAsync(int userId)
        {
            return await _rpEmployee.GetByIdAsync(userId);
        }

        public async Task<int> CreateAsync(UserCreateModel model)
        {
            if (model == null)
            {
                return ToResponse(0, Errors.invalid_data);
            }
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                return ToResponse(0, "Tên đăng nhập không được để trống");
            }
            if (model.UserName.Contains(" "))
            {
                return ToResponse(0, "Tên đăng nhập viết liền không dấu cách");
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return ToResponse(0, "Mật khẩu không được để trống");
            }
            if (model.Password.Trim().Length < 5)
            {
                return ToResponse(0, "Mật khẩu phải có ít nhất 5 ký tự");
            }
            if (string.IsNullOrWhiteSpace(model.PasswordConfirm))
            {
                return ToResponse(0, "Mật khẩu xác thực không được để trống");
            }
            if (model.Password != model.PasswordConfirm)
            {
                return ToResponse(0, "Mật khẩu không khớp");
            }

            if (model.RoleId <=0)
            {
                return ToResponse(0, "Vui lòng chọn vai trò");
            }

            if (!string.IsNullOrWhiteSpace(model.Email) && !BusinessExtensions.IsValidEmail(model.Email.Trim(), 50))
            {
                return ToResponse(0, "Email không hợp lệ");
            }

            if (string.IsNullOrWhiteSpace(model.Code))
            {
                return ToResponse(0, "Mã nhân viên không được để trống");
            }

            var existUserName = await _rpEmployee.GetByUserNameAsync(model.UserName.Trim(), _process.User.Id);
            if (existUserName != null)
            {
                return ToResponse(0, "Tên đăng nhập đã tồn tại");
            }
            var existCode = await _rpEmployee.GetByCodeAsync(model.Code.Trim(), _process.User.Id);
            if (existCode != null)
            {
                return ToResponse(0, "Mã đã tồn tại");
            }

            var user = _mapper.Map<UserSql>(model);
            user.CreatedBy = _process.User.Id;
            user.Code = user.Code.Trim().ToLower();
            user.Email = user.Email.Trim().ToLower();
            user.UserName = user.UserName.Trim().ToLower();
            user.Password = user.Password.Trim();
            user.Password = Utils.getMD5(model.Password);

            var response = await _rpEmployee.CreateAsync(user);
            return ToResponse(response);
        }

        public async Task<List<OptionSimple>> GetByProvinceIdAsync(int provinceId)
        {
            var result = await _rpEmployee.GetByProvinceIdAsync(provinceId);
            return result;
        }
        public async Task<List<OptionSimple>> GetSalesAsync()
        {
            var result = new List<OptionSimple>();
            var groups = await _rpGroup.GetGroupByUserId(_process.User.Id);
            if (groups != null)
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    var members = await _rpEmployee.GetMemberByGroupIdIncludeChildAsync(groups[i].Id, _process.User.Id);
                    if (members != null)
                        result.AddRange(members);

                }
                result.DistinctBy(p => p.Id);
            }

            return result;
        }
        public async Task<List<OptionSimple>> GetMemberByGroupIdAsync(int groupId)
        {
            if (groupId > 0)
                return await _rpEmployee.GetMemberByGroupIdIncludeChildAsync(groupId, _process.User.Id);
            var result = new List<OptionSimple>();
            var groups = await _rpGroup.GetGroupByUserId(_process.User.Id);
            if (groups != null)
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    var members = await _rpEmployee.GetMemberByGroupIdAsync(groups[i].Id, _process.User.Id);
                    if (members != null)
                        result.AddRange(members);

                }
                result.DistinctBy(p => p.Id);
            }
            return result;
        }

        public async Task<bool> GetStatusAsync(int userId)
        {
            return await _rpEmployee.GetStatusAsync(userId);
        }


        public async Task<Account> LoginAsync(LoginModel model)
        {
            if (model == null)
            {
                AddError(Errors.invalid_data);
                return null;
            }

            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
            {
                //AddError($"connStr:{_rpEmployee.ConnectStr}, username: {username}, password: {password}");
                AddError(Errors.username_or_password_must_not_be_empty);
                return null;
            }
            var encodePass = Utils.getMD5(model.Password);

            var account = await _rpEmployee.LoginAsync(model.UserName, encodePass);

            if (account == null)
            {
                AddError(Errors.invalid_username_or_pass);
                return null;
            }
            account.Permissions = await _rpEmployee.GetPermissionsAsync(account.RoleCode);
            return account;
        }

        public async Task<List<OptionSimple>> GetAllEmployeePagingAsync(int page, string freeText)
        {
            var result = await _rpEmployee.GetAllEmployeePagingAsync(_process.User.OrgId, page, freeText);
            return result;
        }

        public async Task<List<OptionSimple>> GetAllEmployeeAsync()
        {
            var result = await _rpEmployee.GetAllEmployeeAsync(_process.User.OrgId);
            return result;
        }

        public async Task<DataPaging<List<EmployeeViewModel>>> SearchsAsync(int role, string freeText, int page = 1, int limit = 10)
        {
            var data = await _rpEmployee.GetsAsync(role, freeText, page, limit, _process.User.OrgId);
            if (data == null || !data.Any())
                return DataPaging.Create<List<EmployeeViewModel>>(null, 0);
            return DataPaging.Create<List<EmployeeViewModel>>(data, data[0].TotalRecord);
        }

        public async Task<List<OptionSimple>> GetRoleListAsync()
        {
            var result = await _rpEmployee.GetRoleListAsync(_process.User.Id);
            return result;
        }
    }
}
