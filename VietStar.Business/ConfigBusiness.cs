using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
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


    }
}
