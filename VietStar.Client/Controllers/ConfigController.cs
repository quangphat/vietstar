using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using McreditServiceCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    [Authorize]
    [Route("Config")]
    public class ConfigController : VietStarBaseController
    {
        protected readonly IConfigBusiness _bizConfig;
        protected readonly IMCreditService _svMcredit;
        public ConfigController(IConfigBusiness configBusiness,
            IMCreditService mCreditService,
            CurrentProcess process) : base(process)
        {
            _bizConfig = configBusiness;
            _svMcredit = mCreditService;
        }

        [MyAuthorize(Permissions ="mcredit.logsign.update")]
        [HttpGet("UpdateLogSign")]
        public IActionResult UpdateLogSign()
        {
            return View();
        }

        [MyAuthorize(Permissions = "mcredit.logsign.update")]
        [HttpPost("UpdateLogSign/{id}")]
        public async Task<IActionResult> UpdateFromMcAsync(int id)
        {
            var result = await _svMcredit.AuthenByUserId(_process.User.Id, new int[] { id });
            return View();
        }
    }
}