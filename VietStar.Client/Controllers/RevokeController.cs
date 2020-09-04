using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Collection;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    [Authorize]
    public class RevokeController : VietStarBaseController
    {
        protected readonly IRevokeDebtBusiness _bizRevoke;
        protected readonly IHostingEnvironment _hosting;
        public RevokeController(IRevokeDebtBusiness revokeBusiness, IHostingEnvironment hosting, CurrentProcess process) : base(process)
        {
            _bizRevoke = revokeBusiness;
            _hosting = hosting;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(string freeText = null,
            string status = null,
            int groupId = 0,
            int assigneeId = 0,
            int page = 1,
            int limit = 10,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int dateType = 1,
            int processStatus = -1)
        {
            var result = await _bizRevoke.SearchAsync(fromDate, toDate, dateType, groupId, assigneeId, status, processStatus, freeText, page, limit);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "revoke,revoke.import")]
        [HttpPost("revoke/import")]
        public async Task<IActionResult> Import()
        {
            var file = Request.Form.Files.FirstOrDefault();
            var result = await _bizRevoke.InsertFromFileAsync(file);
            return ToResponse(result);
        }

        [HttpGet("revoke/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var profile = await _bizRevoke.GetByIdAsync(id);
            return View(profile);
        }

        [HttpPut("revoke/update/{profileId}")]
        public async Task<IActionResult> Update(int profileId, [FromBody] RevokeSimpleUpdate model)
        {

            var result = await _bizRevoke.UpdateSimpleAsync(model, profileId);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "revoke.delete")]
        [HttpDelete("revoke/{profileId}")]
        public async Task<IActionResult> Delete(int profileId)
        {

            var result = await _bizRevoke.DeleteByIdAsync(profileId);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "revoke,revoke.export")]
        [HttpGet("revoke/export")]
        public async Task<IActionResult> Export(string freeText = null,
            string status = null,
            int groupId = 0,
            int assigneeId = 0,
            int page = 1,
            int limit = 10,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int dateType = 1,
            int processStatus = -1)
        {
            var result = await _bizRevoke.ExportAsync(_hosting.ContentRootPath, fromDate, toDate, dateType, groupId, assigneeId, status, processStatus, freeText, page, limit);

            return ToResponse(result);
        }
    }
}