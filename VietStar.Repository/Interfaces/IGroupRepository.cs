using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Commons;
using VietStar.Entities.GroupModels;

namespace VietStar.Repository.Interfaces
{
    public interface IGroupRepository
    {
        Task<List<GroupModel>> GetParentGroupsByUserIdAsync(int userId);
        Task<List<GroupModel>> GetGroupByUserId(int userId);
        Task<List<GroupModel>> GetChildGroupBaseParentSequenceCodeByParentId(int parentGroupId, int userId);
        Task<List<GroupModel>> GetParentGroupsAsync(int userId);
        Task<List<GroupModel>> GetChildGroupByParentIdAsync(int parentGroupId, int userId);
        Task<List<GroupIndexModel>> GetChildGroupByParentIdForPagingAsync(int page, int limit, int parentGroupId, int userId);
        Task<GroupModel> GetGroupByIdAsync(int groupId);
        Task<BaseResponse<bool>> UpdateAsync(GroupEditModel model, string parentSequenceCode, int orgId);
        Task<BaseResponse<string>> GetParentSequenceCodeAsync(int groupId);
        Task<BaseResponse<bool>> CreateConfigAsync(int userId, List<int> groupIds);
        Task<int> GetGroupIdByLeaderIdAsync(int leaderId);
    }
}
