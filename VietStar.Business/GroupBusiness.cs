using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.GroupModels;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

namespace VietStar.Business
{
    public class GroupBusiness : BaseBusiness, IGroupBusiness
    {
        protected readonly IGroupRepository _rpGroup;
        protected readonly IEmployeeRepository _rpEmployee;
        public GroupBusiness(IGroupRepository groupRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpGroup = groupRepository;
            _rpEmployee = employeeRepository;
        }

        public async Task<List<GroupModel>> GetApproveGroupByUserId()
        {
            var groups = await _rpGroup.GetParentGroupsByUserIdAsync(_process.User.Id);
            return await GetGroupByUserId(groups);
        }
        public async Task<List<GroupModel>> GetGroupByUserId()
        {
            var groups = await _rpGroup.GetGroupByUserId(_process.User.Id);
            return await GetGroupByUserId(groups);
        }
        public async Task<List<GroupModel>> GetParentGroups()
        {
            var result = await _rpGroup.GetParentGroupsAsync(_process.User.Id);
            return GenerateChildList(result, "0");
        }
        private async Task<List<GroupModel>> GetGroupByUserId(List<GroupModel> groups)
        {

            if (groups == null || !groups.Any())
                return null;
            var result = new List<GroupModel>();
            var temp = new List<GroupModel>();
            foreach (var group in groups)
            {
                if (temp.FirstOrDefault(p => group.ParentSequenceCode.Contains($".{p.Id}.") || group.ParentSequenceCode.EndsWith($".{p.Id.ToString()}")) != null)
                    continue;
                temp.RemoveAll(p => group.ParentSequenceCode.Contains($".{p.Id}.") || group.ParentSequenceCode.EndsWith($".{p.Id.ToString()}"));
                temp.Add(group);
                if (temp.Count > 0)
                {
                    foreach (var item in temp)
                    {
                        var children = await _rpGroup.GetChildGroupBaseParentSequenceCodeByParentId(item.Id, _process.User.Id);
                        if (children != null || children.Any())
                        {
                            result.AddRange(GenerateChildList(children, $"{item.ParentSequenceCode}.{item.Id}", _process.User.Id));
                        }
                    }

                }
            }
            return result;
        }

        private List<GroupModel> GenerateChildList(List<GroupModel> groups, string parentSequenceCode, int leaderId = 0)
        {
            if (groups == null || !groups.Any())
                return null;
            var lstResult = new List<GroupModel>();
            var stack = new Stack<GroupModel>();
            var lstFind = new List<GroupModel>();
            string parentCodeTemp = parentSequenceCode;
            do
            {
                if (stack.Count > 0)
                {
                    var nhom = stack.Pop();
                    if (nhom != null)
                    {
                        string[] tempArray = nhom.ParentSequenceCode.Split('.');
                        if (tempArray.Length > 1)
                        {
                            for (int i = 0; i < tempArray.Length - 1; i++)
                            {
                                nhom.Name = "-" + nhom.Name;
                            }
                        }
                        lstResult.Add(nhom);
                        parentCodeTemp = nhom.ParentSequenceCode + "." + nhom.Id;
                    }
                }
                lstFind = groups.FindAll(x => x.ParentSequenceCode.Equals(parentCodeTemp));
                if (lstFind != null)
                {
                    for (int i = lstFind.Count - 1; i >= 0; i--)
                    {
                        if (leaderId > 0)
                        {
                            if (lstFind[i].ParentSequenceCode == parentSequenceCode && lstFind[i].LeaderId != leaderId)
                                continue;
                        }

                        stack.Push(lstFind[i]);
                        groups.Remove(lstFind[i]);
                    }
                }
            } while (stack.Count > 0);
            return lstResult;
        }

        public async Task<List<GroupModel>> GetChildByParentIdAsync(int parentId)
        {
            var result = await _rpGroup.GetChildGroupByParentIdAsync(parentId, _process.User.Id);
            return result;
        }
        public async Task<DataPaging<List<GroupIndexModel>>> SearchAsync(int parentId, int page = 1, int limit = 10)
        {
            var data = await _rpGroup.GetChildGroupByParentIdForPagingAsync(page, limit, parentId, _process.User.Id);
            if (data == null || !data.Any())
                return DataPaging.Create<List<GroupIndexModel>>(null, 0);
            return DataPaging.Create<List<GroupIndexModel>>(data, data.FirstOrDefault().TotalRecord);
        }

        public async Task<GroupModel> GetGroupByIdAsync(int groupId)
        {
            var result = await _rpGroup.GetGroupByIdAsync(groupId);
            return result;
        }

        public async Task<List<OptionSimple>> GetMemberByGroupIdAsync(int groupId)
        {
            var result = await _rpEmployee.GetMemberByGroupIdAsync(groupId, _process.User.Id);
            return result;
        }

        public async Task<bool> UpdateAsync(GroupEditModel model)
        {
            if (model == null || model.Id <= 0)
                return ToResponse(false, Errors.invalid_data);

            if (model.MemberIds == null)
                model.MemberIds = new List<int>();

            string parentSequenceCode = "0";
            if (model.ParentId > 0)
            {
                var parentCode = _rpGroup.GetParentSequenceCodeAsync(model.ParentId);
                parentSequenceCode = parentCode + "." + model.ParentId;
            }

            return ToResponse(await _rpGroup.UpdateAsync(model, parentSequenceCode, _process.User.OrgId));
        }

        public async Task<bool> CreateConfigAsync(CreateConfigModel model)
        {
            if(model==null)
            {
                return ToResponse(false, Errors.invalid_data);
            }
            if(model.UserId<=0)
            {
                return ToResponse(false, "Vui lòng chọn nhân viên");
            }
            if (model.GroupIds ==null|| !model.GroupIds.Any())
            {
                return ToResponse(false, "Vui lòng chọn nhóm");
            }
            return ToResponse(await _rpGroup.CreateConfigAsync(model.UserId, model.GroupIds));
        }
    }
}
