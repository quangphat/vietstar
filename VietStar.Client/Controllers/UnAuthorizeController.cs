using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VietStar.Client.Controllers
{
    [AllowAnonymous]
    public class UnAuthorizeController : Controller
    {
        public IActionResult UnAuthorize()
        {
            return View();
        }
    }
}