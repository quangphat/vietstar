using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VietStar.Client.Controllers
{
    [AllowAnonymous]
    public class ErrorPageController : Controller
    {
        public IActionResult UnAuthorized(string unAuthorizedUrl)
        {
            ViewBag.unAuthorizedUrl = unAuthorizedUrl;
            return View();
        }
    }
}