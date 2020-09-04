using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    public class PartnerController : VietStarBaseController
    {
        protected readonly IPartnerBusiness _bizPartner;
        public PartnerController(IPartnerBusiness partnerBusiness, CurrentProcess process) : base(process)
        {
            _bizPartner = partnerBusiness;
        }
        [MyAuthorize(Permissions = "profile")]
        public IActionResult Index()
        {
            return View();
        }
    }
}