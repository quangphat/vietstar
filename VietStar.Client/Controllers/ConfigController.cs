using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using McreditServiceCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Mcredit;

namespace VietStar.Client.Controllers
{
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

        public async Task<IActionResult> AuthenMC([FromBody]AuthenMcRequestModel model)
        {
            if (model == null)
                return ToResponse(false);
            var result = await _svMcredit.AuthenByUserId(model.UserId, model.TableToUpdateIds);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions ="mcredit.logsign.update")]
        [HttpGet("UpdateLogSign")]
        public IActionResult UpdateLogSign()
        {
            return View();
        }

        [MyAuthorize(Permissions = "mcredit.product.update")]
        [HttpGet("UpdateProduct")]
        public IActionResult UpdateProduct()
        {
            return View();
        }

        [MyAuthorize(Permissions = "mcredit.loanperiod.update")]
        [HttpGet("UpdateloanPeriod")]
        public IActionResult UpdateloanPeriod()
        {
            return View();
        }

        [MyAuthorize(Permissions = "mcredit.logsign.update")]
        [HttpPost("UpdateLogSign/{id}")]
        public async Task<IActionResult> UpdateFromMcAsync(int id)
        {
            var result = await _svMcredit.AuthenByUserId(_process.User.Id, new int[] { id });
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "secret")]
        [HttpGet("ExcuteSql")]
        public IActionResult ExcuteQuery()
        {
            return View();
        }

        [MyAuthorize(Permissions = "secret")]
        [HttpPost("ExcuteSql")]
        public async Task<IActionResult> ExcuteSqlAsync([FromBody]StringModel model)
        {
            var result = await _bizConfig.ExcuteQueryAsync(model);
            return ToResponse(result);
        }
    }
}