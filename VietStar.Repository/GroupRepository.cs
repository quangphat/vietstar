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
using VietStar.Entities.GroupModels;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class GroupRepository : RepositoryBase, IGroupRepository
    {
        protected readonly ILogRepository _rpLog;
        public GroupRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }

        public async Task<List<GroupModel>> GetChildGroupBaseParentSequenceCodeByParentId(int parentGroupId, int userId)
        {
            try
            {
                using (var _con = GetConnection())
                {
                    //old name: sp_NHOM_LayCayNhomCon_v3
                    var result = await _con.QueryAsync<GroupModel>("sp_Gorup_GetChildByParentSequenceCode", new { parentGroupId, userId }, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception e)
            {
                await _rpLog.InsertLogFromException(nameof(GetChildGroupBaseParentSequenceCodeByParentId), e);
                return null;
            }
        }

        public async Task<List<GroupIndexModel>> GetChildGroupByParentIdForPagingAsync(int page, int limit, int parentGroupId, int userId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    ProcessInputPaging(ref page, ref limit, out int offset);
                    var result = await con.QueryAsync<GroupIndexModel>("sp_Group_GetChildGroupForPaging", new { parentGroupId, userId, page, limit }, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<GroupModel>> GetChildGroupByParentIdAsync(int parentGroupId, int userId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<GroupModel>("sp_Group_GetChildGroup_v2", new { parentGroupId, userId }, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<GroupModel>> GetGroupByUserId(int userId)
        {
            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.QueryAsync<GroupModel>("sp_NHOM_LayDSChonTheoNhanVien_v3", new { userId }, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception e)
            {
                await _rpLog.InsertLogFromException(nameof(GetGroupByUserId), e);
                return null;
            }
        }
        public async Task<List<GroupModel>> GetParentGroupsByUserIdAsync(int userId)
        {
            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.QueryAsync<GroupModel>("sp_NHOM_LayDSNhomDuyetChonTheoNhanVien_v3", new { userId }, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception e)
            {
                await _rpLog.InsertLogFromException(nameof(GetGroupByUserId), e);
                return null;
            }
        }
        public async Task<List<GroupModel>> GetParentGroupsAsync(int userId)
        {
            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.QueryAsync<GroupModel>("sp_NHOM_LayDSNhom_v2", new { userId }, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception e)
            {
                await _rpLog.InsertLogFromException(nameof(GetGroupByUserId), e);
                return null;
            }
        }

        public async Task<GroupModel> GetGroupByIdAsync(int groupId)
        {
            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.QueryFirstOrDefaultAsync<GroupModel>("sp_NHOM_LayThongTinTheoMa_v2", new { groupId }, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception e)
            {
                await _rpLog.InsertLogFromException(nameof(GetGroupByUserId), e);
                return null;
            }
        }
        public async Task<BaseResponse<bool>> UpdateAsync(GroupEditModel model, string parentSequenceCode, int orgId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.ExecuteAsync("sp_Group_Update",
                        new
                        {
                            Id = model.Id,
                            parentId = model.ParentId,
                            LeaderId = model.LeaderId,
                            ShortName = model.ShortName,
                            Name = model.Name,
                            parentSequenceCode,
                            orgId,
                            memberIds = string.Join(',', model.MemberIds)
                        },
                        commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }

            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Create(false, GetException(ex));
            }
        }

        public async Task<BaseResponse<string>> GetParentSequenceCodeAsync(int groupId)
        {

            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryFirstOrDefaultAsync<string>("sp_NHOM_LayChuoiMaChaCuaMaNhom",
                        new
                        {
                            MaNhom = groupId
                        },
                        commandType: CommandType.StoredProcedure);
                    return BaseResponse<string>.Create(result);
                }

            }
            catch (Exception ex)
            {
                return BaseResponse<string>.Create("0", GetException(ex));
            }
        }

        public async Task<BaseResponse<bool>> CreateConfigAsync(int userId, List<int> groupIds)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.ExecuteAsync("sp_Group_UpdateConfig",
                        new
                        {
                            userId,
                            groupIds = string.Join(',', groupIds)
                        },
                        commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }

            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Create(false, GetException(ex));
            }
        }

        public async Task<int> GetGroupIdByLeaderIdAsync(int leaderId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<int>("sp_GetMaNhomChaByMaNguoiQuanLy", new { leaderId },
                    commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}

