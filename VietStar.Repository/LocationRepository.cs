using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.Commons;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class LocationRepository : RepositoryBase, ILocationRepository
    {
        protected readonly ILogRepository _rpLog;
        public LocationRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }
        public async Task<List<OptionSimple>> GetProvincesAync()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_KHU_VUC_LayDSTinh_v2", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimple>> GetDistrictsAync(int provinceId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_KHU_VUC_LayDSHuyen_v2",new { provinceId}, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}

