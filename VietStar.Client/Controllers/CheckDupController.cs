using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.CheckDup;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    [Authorize]
    public class CheckDupController : VietStarBaseController
    {
        protected readonly ICheckDupBusiness _bizCheckDup;
        public CheckDupController(ICheckDupBusiness checkdupBusiness,CurrentProcess process) : base(process)
        {
            _bizCheckDup = checkdupBusiness;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Search(string freeText = null, int page = 1, int limit = 10)
        {
            var result = await _bizCheckDup.GetsAsync(freeText, page, limit);
            return ToResponse(result);
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> CreateAsync([FromBody] CheckDupAddModel model)
        {
            var result = await _bizCheckDup.CreateAsync(model);
            return ToResponse(result);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var result = await _bizCheckDup.GetByIdAsync(id);
            return View(result);
        }
        public async Task<IActionResult> UpdateAsync([FromBody] CheckDupEditModel model)
        {
            var result = await _bizCheckDup.UpdateAsync(model);
            return ToResponse(result);
        }
        public async Task<IActionResult> GetNotes(int id)
        {
            var result = await _bizCheckDup.GetNotesAsync(id);
            return ToResponse(result);
        }
    }
}