using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;

namespace KingOffice.Infrastructures
{
    public class SessionHandler
    {
        private readonly RequestDelegate _next;
        internal const string SESSION_KEY = "current-user";
        public SessionHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext, CurrentProcess process, IEmployeeRepository rpEmployee)
        {
            var raw = httpContext.Session.Get(SESSION_KEY);
            if (raw == null)
            {
                var user = httpContext.GetUserInfo();
                if (user == null)
                {
                    await _next(httpContext);
                    return;
                }
                raw = Utils.ToBinary(user);
                httpContext.Session.Set(SESSION_KEY, raw);
            }
            var account = Utils.FromBinary(raw);
            var userInSql = await rpEmployee.GetByIdAsync(account.Id);
            if((userInSql == null  || userInSql.FirstLogin))
            {
                httpContext.Response.Redirect("/Account/Login");
                return;
            }
            if(userInSql.IsDeleted && httpContext.Request.Path.Value != "/Account/Login")
            {
                httpContext.Response.Redirect("/Account/Login");
                return;
            }
            account.IsActive = !userInSql.IsDeleted;
            process.User = account;
            CurrentProcess.CurrentUser = account;
            await _next(httpContext);
        }
    }
}
