using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Profile;
using VietStar.Entities.ViewModels;

namespace VietStar.Client.Controllers
{
    public class ProfileController : VietStarBaseController
    {
        protected readonly IProfileBusiness _bizProfile;
        protected readonly IHostingEnvironment _hosting;
        public ProfileController(IProfileBusiness profileBusiness,IHostingEnvironment hosting,CurrentProcess process) : base(process)
        {
            _bizProfile = profileBusiness;
            _hosting = hosting;
        }
        [MyAuthorize(Permissions ="profile,profile.view")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1
            , int groupId = 0
            , int memberId = 0
            , string status = null
            , string freeText = null
            , int page = 1
            , int limit = 20
            , string sort ="desc"
            , string sortField = "updatedTime")
        {
            //return ToResponse(new List<ProfileIndexModel> { new ProfileIndexModel { TotalRecord = 100 } });
            var result = await _bizProfile.GetsAsync(fromDate, toDate, dateType, groupId, memberId, status, freeText,sort, sortField, page, limit);

            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "profile,profile.export")]
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
            var result = await _bizProfile.ExportAsync(_hosting.ContentRootPath,fromDate, toDate, dateType, groupId, memberId, status, freeText, sort, sortField, page, limit);

            return ToResponse(result);
        }

        public IActionResult Approve()
        {
            return View();
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost("profile/CreateAsync")]
        public async Task<IActionResult> CreateAsync([FromBody]ProfileAdd model)
        {
            var result = await _bizProfile.CreateAsync(model);
            return ToResponse(result);
        }
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _bizProfile.GetByIdAsync(id);
            
            return View(result ?? new ProfileEditView());
        }
        [Authorize]
        [HttpPost("profile/update")]
        public async Task<IActionResult> UpdateAsync([FromBody]ProfileAdd model)
        {
            var result = await _bizProfile.UpdateProfile(model);
            return ToResponse(result);
        }
    }
}