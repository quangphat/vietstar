using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

namespace VietStar.Repository
{
    public class LogRepository : RepositoryBase, ILogRepository
    {
        public LogRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task InsertLog(string name, string content)
        {
            using (var _con = GetConnection())
            {
                await _con.ExecuteAsync("sp_InsertLog", new { name, content }, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task InsertLogFromException(string name, Exception e)
        {
            using (var _con = GetConnection())
            {
                var content = e.InnerException == null ? e.Dump() : e.InnerException.Dump();
                await _con.ExecuteAsync("sp_InsertLog", new { name, content }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}

