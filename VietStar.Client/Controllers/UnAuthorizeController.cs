using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Entities.Infrastructures;
using VietStar.Repository.Interfaces;

namespace VietStar.Client.Controllers
{
    [AllowAnonymous]
    public class ErrorPageController : Controller
    {
        private readonly ILogRepository _rpLog;
        public ErrorPageController(ILogRepository logRepository)
        {
            _rpLog = logRepository;
        }
        public async Task<IActionResult> UnAuthorized(string unAuthorizedUrl)
        {
            ViewBag.unAuthorizedUrl = unAuthorizedUrl;
            string msg = $"username: {CurrentProcess.CurrentUser.UserName} - Id: {CurrentProcess.CurrentUser.Id} - roleCode: {CurrentProcess.CurrentUser.RoleCode}";
            ViewBag.user = msg;
            await _rpLog.InsertLog("unauthorized", msg);
            return View();
        }
    }
}