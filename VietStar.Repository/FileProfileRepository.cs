using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities;
using VietStar.Entities.FileProfile;
using VietStar.Entities.Mcredit;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class FileProfileRepository : RepositoryBase, IFileProfileRepository
    {
        protected readonly ILogRepository _rpLog;
        public FileProfileRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }
        public async Task<List<FileUploadModel>> GetFilesByProfileIdAsync(int profileType, int profileId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("profileId", profileId);
                p.Add("profileTypeId", profileType);
                var result = await con.QueryAsync<FileUploadModel>("getTailieuByHosoId", p,
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<FileProfileType>> GetByType(int profileType)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<FileProfileType>("sp_LOAI_TAI_LIEU_GetsByType", new {profileType }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<BaseResponse<int>> AddMCredit(MCProfileFileSqlModel model)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var p = GetParams(model, outputParam: "Id");
                    await con.ExecuteAsync("sp_TAI_LIEU_HS_Them_v2", p,
                        commandType: CommandType.StoredProcedure);
                    return BaseResponse<int>.Create(p.Get<int>("Id"));
                }
            }
            catch (Exception e)
            {
                return BaseResponse<int>.Create(0, GetException(e));
            }

        }

        public async Task<BaseResponse<int>> Add(ProfileFileAddSql model)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var p = AddOutputParam("Id");
                    p.Add("FileKey", model.FileKey);
                    p.Add("FilePath", model.FilePath);
                    p.Add("Folder", model.Folder);
                    p.Add("FileName", model.FileName);
                    p.Add("ProfileId", model.ProfileId);
                    p.Add("ProfileTypeId", model.ProfileTypeId);
                    p.Add("GuidId", model.GuidId);
                    p.Add("FileId", model.FileId);
                    await con.ExecuteAsync("sp_TAI_LIEU_HS_Them_v2", p,
                        commandType: CommandType.StoredProcedure);
                    var id = p.Get<int>("Id");
                    return BaseResponse<int>.Create(id);
                }
            }
            catch(Exception e)
            {
                return BaseResponse<int>.Create(0, GetException(e));
            }
            

        }

        public async Task<bool> DeleteByIdAsync(int fileId, string guidId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_ProfileFileDeleteById", new { fileId, guidId }, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
            catch(Exception e)
            {
                return false;
            }
           
        }

        public async Task<bool> UpdateFileMCProfileByIdAsync(int profileId, string mcId)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_TaiLieuHoso_UpdateMCId", new
                {
                    profileId,
                    mcId
                },
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        //public async Task<bool> UpdateExistingFile(FileUploadModel model, int fileId)
        //{
        //    var p = GetParams(model, ignoreKey: new string[] { nameof(model.FileKey), nameof(model.ProfileId) });
        //    p.Add("fileId", fileId);
        //    using (var con = GetConnection())
        //    {
        //        var result = await con.ExecuteAsync("updateExistingFile", p,
        //            commandType: CommandType.StoredProcedure);
        //        return true;
        //    }
        //}
    }
}

