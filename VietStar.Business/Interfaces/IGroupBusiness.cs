using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Commons;
using VietStar.Entities.GroupModels;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface IGroupBusiness
    {
        Task<List<GroupModel>> GetChildByParentIdAsync(int parentId);
        Task<List<GroupModel>> GetApproveGroupByUserId();
        Task<List<GroupModel>> GetGroupByUserId();
        Task<List<GroupModel>> GetParentGroups();
        Task<DataPaging<List<GroupIndexModel>>> SearchAsync(int parentId, int page = 1, int limit = 10);
        Task<GroupModel> GetGroupByIdAsync(int groupId);
        Task<List<OptionSimple>> GetMemberByGroupIdAsync(int groupId);
        Task<bool> UpdateAsync(GroupEditModel model);
        Task<bool> CreateConfigAsync(CreateConfigModel model);
    }
}
