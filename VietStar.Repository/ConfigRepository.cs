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


namespace VietStar.Repository
{
    public class ConfigRepository : RepositoryBase, IConfigRepository
    {
	protected readonly ILogRepository _rpLog;
        public ConfigRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
		_rpLog = logRepository;
        }
    }
}

