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
    public class PartnerRepository : RepositoryBase, IPartnerRepository
    {
        protected readonly ILogRepository _rpLog;
        public PartnerRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }
        public async Task<List<OptionSimple>> GetsAync(int orgId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_DOI_TAC_LayDS_v2", new { orgId }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}

