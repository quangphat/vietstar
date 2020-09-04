using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Courier;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    [Authorize]
    public class CourierController : VietStarBaseController
    {
        protected readonly ICourierBusiness _bizCourier;
        protected readonly IHostingEnvironment _hosting;
        public CourierController(ICourierBusiness courierBusiness, IHostingEnvironment hosting, CurrentProcess process) : base(process)
        {
            _bizCourier = courierBusiness;
            _hosting = hosting;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(string freeText,
            DateTime? fromDate
            , DateTime? toDate
            , int dateType = 2
            , string status = null
            , int page = 1
            , int limit = 10
            , int assigneeId = 0
            , int groupId = 0
            , int provinceId = 0
            , string saleCode = null)
        {
            var result = await _bizCourier.GetsAsync(freeText, fromDate, toDate, dateType, status, page, limit, assigneeId, groupId, provinceId, saleCode);
            return ToResponse(result);
        }

        public async Task<IActionResult> Export(string freeText,
            DateTime? fromDate
            , DateTime? toDate
            , int dateType = 2
            , string status = null
            , int page = 1
            , int limit = 10
            , int assigneeId = 0
            , int groupId = 0
            , int provinceId = 0
            , string saleCode = null
            )
        {
            var result = await _bizCourier.ExportAsync(_hosting.ContentRootPath, freeText, fromDate, toDate, dateType, status, page, limit, assigneeId, groupId, provinceId, saleCode);

            return ToResponse(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> CreateAsync([FromBody] CourierAddModel model)
        {
            var result = await _bizCourier.CreateAsync(model);
            return ToResponse(result);

        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _bizCourier.GetByIdAsync(id);
            ViewBag.isAdmin = _process.User.isAdmin;
            return View(model);
        }

        public async Task<IActionResult> UpdateAsync([FromBody] CourierUpdateModel model)
        {
            var result = await _bizCourier.UpdateAsync(model);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "courier,courier.import")]
        [HttpPost("courier/import/{groupId}")]
        public async Task<IActionResult> Import(int groupId)
        {
            var file = Request.Form.Files.FirstOrDefault();
            var result = await _bizCourier.InsertFromFileAsync(file);
            return ToResponse(result);
        }
    }
}