using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Mcredit;
using McreditServiceCore;
using McreditServiceCore.Interfaces;
using VietStar.Entities.Commons;
using Microsoft.AspNetCore.Hosting;

namespace VietStar.Client.Controllers
{
    [Authorize]
    public class MCreditController : VietStarBaseController
    {
        protected readonly IMCreditBusiness _bizMCredit;
        protected readonly IMCreditService _svMcredit;
        protected readonly IHostingEnvironment _hosting;
        public MCreditController(IMCreditBusiness mcreditBusiness
            , IHostingEnvironment hosting,
            IMCreditService mCreditService,
            CurrentProcess process) : base(process)
        {
            _bizMCredit = mcreditBusiness;
            _svMcredit = mCreditService;
            _hosting = hosting;
        }

        public async Task<IActionResult> AuthenMC(AuthenMcRequestModel model)
        {
            if (model == null)
                return ToResponse(false);
            var result = await _svMcredit.AuthenByUserId(model.UserId, model.TableToUpdateIds);
            return ToResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> CheckSaleApi([FromBody]CheckSaleModel model)
        {
            var result = await _bizMCredit.CheckSaleAsync(model);
            return ToResponse(result);
        }

        public IActionResult CheckCat()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckCatApi([FromBody]StringModel model)
        {
            var result = await _bizMCredit.CheckCatAsync(model);
            return ToResponse(result);
        }

        public IActionResult CheckCIC()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckCICApi([FromBody]CheckCICModel model)
        {

            var result = await _bizMCredit.CheckCICAsync(model);
            return ToResponse(result);
        }

        public IActionResult CheckDuplicate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckDupApi([FromBody]StringModel model)
        {

            var result = await _bizMCredit.CheckDupAsync(model);
            return ToResponse(result);
        }

        public IActionResult CheckStatus()
        {
            return View();
        }

        public async Task<IActionResult> CheckStatusApi([FromBody]StringModel model)
        {
            var result = await _bizMCredit.CheckStatusAsync(model);
            return ToResponse(result);
        }

        public IActionResult Temp()
        {
            return View();
        }

        public async Task<IActionResult> SearchTemps(DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1,
            string freeText = null,
            string status = null,
            int page = 1,
            int limit = 20)
        {
            var result = await _bizMCredit.SearchsTemsAsync(fromDate,toDate, dateType ,freeText, status, page, limit);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "mcredit-profile")]
        public IActionResult Index()
        {
            return View();
        }

        [MyAuthorize(Permissions = "mcprofile")]
        public async Task<IActionResult> Search(string freeText, string status, string type, int page = 1)
        {
            var result = await _bizMCredit.SearchsAsync(freeText, status, type, page);
            return ToResponse(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> CreateDraft([FromBody]MCredit_TempProfileAddModel model)
        {
            var result = await _bizMCredit.CreateDraftAsync(model);
            return ToResponse(result);
        }

        public async Task<IActionResult> AddRefuseReasonToNote(int profileId, string type)
        {
            var result = await _bizMCredit.AddRefuseReasonToNoteAsync(profileId, type);
            return ToResponse(result);

        }

        public async Task<IActionResult> UpdateTempProfileStatus(int profileId)
        {
            var result = await _bizMCredit.UpdateTempProfileStatusAsync(profileId);
            return ToResponse(result);
        }

        public async Task<IActionResult> UpdateDraft([FromBody]MCredit_TempProfileAddModel model)
        {
            var result = await _bizMCredit.UpdateDraftAsync(model);
            return ToResponse(result);
        }

        public async Task<IActionResult> TempProfile(int id)
        {
            var result = await _bizMCredit.GetTempProfileByIdAsync(id);
            return View(result);
        }
        public async Task<IActionResult> GetMCSimpleList(string type)
        {
            var result = await _bizMCredit.GetSimpleListByTypeAsync(type);
            return ToResponse(result);
        }

        public async Task<IActionResult> ReSendFileToEC(int mcProfileId)
        {
            var result = await _bizMCredit.ReSendFileToECAsync(mcProfileId);
            return ToResponse(result);
        }

        public async Task<IActionResult> GetNotes(int mcProfileId)
        {
            var result = await _bizMCredit.GetNotesAsync(mcProfileId);
            return ToResponse(result);
        }
        public async Task<IActionResult> AddNoteNotes(string mcId, [FromBody]StringModel model)
        {
            var result = await _bizMCredit.AddNoteToMcAsync(mcId, model);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "mcprofile.submit")]
        public async Task<IActionResult> SubmitToMCredit([FromBody]MCredit_TempProfileAddModel model)
        {
            var result = await _bizMCredit.SubmitToMCreditAsync(model);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "mcprofile")]
        public async Task<ActionResult> MCreditProfile(int id)
        {
            var result = await _bizMCredit.GetMCreditProfileByIdAsync(id);
            return View(result);
        }

        [MyAuthorize(Permissions = "mcprofile,mcprofile.export")]
        public async Task<IActionResult> Export(DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1
            , int groupId = 0
            , int memberId = 0
            , string status = null
            , string freeText = null
            , int page = 1
            , int limit = 20
            , string sort = "desc"
            , string sortField = "updatedTime")
        {
            var result = await _bizMCredit.ExportAsync(_hosting.ContentRootPath, fromDate, toDate, dateType, status, freeText, page, limit);

            return ToResponse(result);
        }
    }
}