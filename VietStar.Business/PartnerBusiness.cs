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
    public class PartnerBusiness : BaseBusiness, IPartnerBusiness
    {
        protected readonly IPartnerRepository _rpPartner;
        public PartnerBusiness(IPartnerRepository partnerRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpPartner = partnerRepository;
        }

       
    }
}
