using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class ProfileNotificationRepository : RepositoryBase, IProfileNotificationRepository
    {
        protected readonly ILogRepository _rpLog;
        public ProfileNotificationRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }
        public async Task<BaseResponse<bool>> CreateAsync(int profileId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_HO_SO_DUYET_XEM_Them", new { ID = profileId }, commandType: CommandType.StoredProcedure);
                    return BaseResponse<bool>.Create(true);
                }
            }
            catch(Exception e)
            {
                await _rpLog.InsertLogFromException($"CreateAsync-ProfileNotification-{profileId}", e);
                return BaseResponse<bool>.Create(false);
            }
        }
    }
}

