using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.ViewModels;

namespace VietStar.Client.Controllers
{
    public class AccountController : VietStarBaseController
    {
        protected readonly IEmployeeBusiness _bizEmployee;
        public AccountController(IEmployeeBusiness employeeBusiness, CurrentProcess process) : base(process)
        {
            _bizEmployee = employeeBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> LoginApi(LoginModel model)
        {
            var account = await _bizEmployee.LoginAsync(model);
            if (account == null || !account.IsActive)
                return ToResponse(false);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Id", account.Id.ToString()));
            claims.Add(new Claim("UserName", account.UserName));
            claims.Add(new Claim("OrgId", account.OrgId.ToString()));
            if (!string.IsNullOrWhiteSpace(account.FullName))
                claims.Add(new Claim("FullName", account.FullName));
            if (!string.IsNullOrWhiteSpace(account.Email))
                claims.Add(new Claim("Email", account.Email));
            if (!string.IsNullOrWhiteSpace(account.Code))
                claims.Add(new Claim("Code", account.Code));
            if (!string.IsNullOrWhiteSpace(account.RoleCode))
                claims.Add(new Claim("RoleCode", account.RoleCode));
            if (account.Permissions.Any())
            {
                claims.Add(new Claim("Scopes", string.Join(",", account.Permissions.ToArray())));
            }

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.Rememberme
            };
            await HttpContext.SignInAsync(principal, authProperties);
            string url = "/Home/Index";
            return ToResponse(url);
        }
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Clear();
            await  HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new SignOutResult(new[] { "Cookies" });
        }
    }
}