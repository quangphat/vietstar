using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

namespace VietStar.Business
{
    public class ConfigBusiness : BaseBusiness, IConfigBusiness
    {
        protected readonly IConfigRepository _rpConfig;
        public ConfigBusiness(IConfigRepository configRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpConfig = configRepository;
        }

        public async Task<List<IDictionary<string, object>>> ExcuteQueryAsync(StringModel model)
        {
            if (_process.User == null)
                return null;
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return null;
            
            var sqltemp = model.Value.ToUpper();
            if (sqltemp.Contains("DELETE ") || sqltemp.Contains("DROP "))
                return null;
            return await _rpConfig.QuerySQLAsync(model.Value);
        }

    }
}
